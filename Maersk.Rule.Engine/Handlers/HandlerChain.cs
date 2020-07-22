using Maersk.Promotion.Engine.Enums;
using Maersk.Rule.Engine.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Maersk.Rule.Engine.Handlers
{
    public class HandlerChain : IHandlerChain
    {
        private IProcessHandler _handler { get; set; }
        private readonly Func<string, IProcessHandler> _serviceAcessor;
        private readonly ILogger _logger;
        public IProcessHandler Handler { get { return _handler; } set { _handler = value; } }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="serviceAccessor"></param>
        /// <param name="settingsManager"></param>
        /// <param name="logger"></param>
        public HandlerChain(Func<string, IProcessHandler> serviceAccessor, ILogger logger)
        {
            _serviceAcessor = serviceAccessor;
            _logger = logger;
        }
        /// <summary>
        /// Registers the pipelinec components
        /// </summary>
        /// <returns></returns>
        public IProcessHandler RegisterOutBoundHandlers(PaymentTypes metaKey)
        {
            string handlerList = string.Empty;
            switch (metaKey)
            {
                //Below configuration can be fetched from any api
                case PaymentTypes.PHYSICALPRODUCT:
                    handlerList = "PhysicalProductPackingSlipHandler,ProcessCommissionPaymentHandler,ResponseHandler"; break;
                case PaymentTypes.BOOKS:
                    handlerList = "CreateDuplicateSlipForRoyaldepartmentHandler,ProcessCommissionPaymentHandler,ResponseHandler"; break;
                case PaymentTypes.MEMBERSHIPACTIVATE:
                    handlerList = "ActivateMemeberShipHandler,NotifyEmailHandler,ResponseHandler"; break;
                case PaymentTypes.MEMBERSHIPUPGRADE:
                    handlerList = "UpgradeMembershipHandler,NotifyEmailHandler,ResponseHandler"; break;
                case PaymentTypes.VIDEO:
                    handlerList = "AddFirstAidVideoHandler,PhysicalProductPackingSlipHandler,ResponseHandler"; break;
                default:
                    return null;
            }
            try
            {
                var outBoundhandlers = handlerList.Split(',');
                outBoundhandlers = outBoundhandlers.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                IProcessHandler currentHandler = null;
                IProcessHandler previoustHandler = null;
                //Create a handler chain execution based on the requirement
                for (int i = outBoundhandlers.Length - 1; i >= 0; i--)
                {
                    //Get the handlers class using named instance 
                    currentHandler = _serviceAcessor(outBoundhandlers[i]);
                    if (i != outBoundhandlers.Length - 1 && previoustHandler != null) 
                        currentHandler.Next(previoustHandler.Invoke);
                    previoustHandler = currentHandler;
                }
                Handler = previoustHandler;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RegisterOutBoundHandlers|Exception:{ex.Message}");
            }

            return Handler;
        }

    }
}
