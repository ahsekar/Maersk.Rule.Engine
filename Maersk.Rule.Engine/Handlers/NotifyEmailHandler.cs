using Maersk.Promotion.Engine.Abstractions;
using Maersk.Rule.Engine.Contracts;
using Maersk.Rule.Engine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Maersk.Rule.Engine.Handlers
{
    public class NotifyEmailHandler : IProcessHandler
    {
        private readonly ILogger _logger;
        private HandlerDelegate _next;
        private readonly IBusinessLogic _manager;
        public NotifyEmailHandler(ILogger logger, IBusinessLogic manager)
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
                var status = _manager.NotifyEmail(httpContext);
                if (status)
                {
                    if (httpContext.Items.ContainsKey("upgrade"))
                        _logger.LogInformation("Email notification sent for upgrade");
                    else if (httpContext.Items.ContainsKey("activate"))
                        _logger.LogInformation("Email notification sent for activate");
                }
                else
                    return new Response("FAILED", "Failed to send email notification");

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception:{e.Message}");
            }

            return _next.Invoke(httpContext);
        }

    }
}
