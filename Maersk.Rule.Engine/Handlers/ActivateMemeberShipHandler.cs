using Maersk.Promotion.Engine.Abstractions;
using Maersk.Rule.Engine.Contracts;
using Maersk.Rule.Engine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Maersk.Rule.Engine.Handlers
{
    public class ActivateMemeberShipHandler : IProcessHandler
    {

        private readonly ILogger _logger;
        private HandlerDelegate _next;
        private readonly IBusinessLogic _manager;
        public ActivateMemeberShipHandler(ILogger logger, IBusinessLogic manager)
        {
            _manager = manager;
            _logger = logger;
        }
        /// <summary>
        /// Delegates to the next handler
        /// </summary>
        /// <param name="next"></param>
        public void Next(HandlerDelegate next)
        {
            _next = next;
        }

        public Response Invoke(HttpContext httpContext)
        {
            try
            {
                var status = _manager.ActivateMembership(httpContext);
                if (status)
                {
                    httpContext.Items.Add("activate", true);
                    _logger.LogInformation("Membership has been activated");
                }
                else
                    return new Response("FAILED", "Failed to activate membership");

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception:{e.Message}");
            }

            return _next.Invoke(httpContext);
        }

    }
}
