using Maersk.Promotion.Engine.Enums;
using Maersk.Rule.Engine.Contracts;
using Maersk.Rule.Engine.Handlers;
using Maersk.Rule.Engine.Models;
using Microsoft.AspNetCore.Http;

namespace Maersk.Rule.Engine.Business
{
    public class ProcessRequestEngine : IProcessRequestEngine
    {
        private readonly IHandlerChain _handlerChain;

        public ProcessRequestEngine(IHandlerChain handlerChain)
        {
            _handlerChain = handlerChain;
        }

        /// <summary>
        /// This class can be called from a middleware for handler registration to execute the flow
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Response RunBusinessRuleEngine(PaymentTypes type)
        {
            var httpContext = new DefaultHttpContext();//Get the httpcontext from incoming request and pass it to the handlers to invoke
            //Based on the incoming request type identifier we can pass different types to the chain of handlers to initialize
            var handlers = _handlerChain.RegisterOutBoundHandlers(type);
            if (handlers != null)
            {
                return handlers.Invoke(httpContext);
            }
            else
                return null;
        }
    }
}
