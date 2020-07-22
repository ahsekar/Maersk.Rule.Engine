using Maersk.Promotion.Engine.Abstractions;
using Maersk.Rule.Engine.Contracts;
using Maersk.Rule.Engine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Maersk.Rule.Engine.Handlers
{
    public class UpgradeMembershipHandler : IProcessHandler
    {
        private readonly IBusinessLogic _manager;
        private readonly ILogger _logger;
        private HandlerDelegate _next;

        public UpgradeMembershipHandler(IBusinessLogic manager, ILogger logger)
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
                var status = _manager.UpgradeMembership(httpContext);
                if (status)
                {
                    _logger.LogInformation("Membership has been upgrade");
                    httpContext.Items.Add("upgrade", true);
                }
                else
                    return new Response("FAILED", "Fail to upgrade membership");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception:{e.Message}");
            }

            return _next.Invoke(httpContext);
        }
    }
}
