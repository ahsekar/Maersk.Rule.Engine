using Maersk.Promotion.Engine.Abstractions;
using Maersk.Rule.Engine.Contracts;
using Maersk.Rule.Engine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Maersk.Rule.Engine.Handlers
{
    public class CreateDuplicateSlipForRoyaldepartmentHandler : IProcessHandler
    {

        private readonly ILogger _logger;
        private HandlerDelegate _next;
        private readonly IBusinessLogic _manager;
        public CreateDuplicateSlipForRoyaldepartmentHandler(ILogger logger, IBusinessLogic manager)
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
                var status = _manager.CreateDuplicateSlip(httpContext);
                if (status)
                    _logger.LogInformation("duplicate slip for royalty department created");
                else
                    return new Response("FAILED", "Faild to creating duplicate slip for royalty department");

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception:{e.Message}");
            }

            return _next.Invoke(httpContext);
        }

    }
}
