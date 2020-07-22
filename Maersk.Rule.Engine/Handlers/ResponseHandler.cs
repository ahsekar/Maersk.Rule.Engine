using Maersk.Promotion.Engine.Abstractions;
using Maersk.Rule.Engine.Contracts;
using Maersk.Rule.Engine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Maersk.Rule.Engine.Handlers
{
    public class ResponseHandler : IProcessHandler
    {
        private readonly ILogger _logger;
        private HandlerDelegate _next;
        public ResponseHandler(ILogger logger)
        {
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
        public Response Invoke(HttpContext context)
        {
            //Final response can be consolidated and send from this handler. ResponseHandler should be there as part of each flow 
            _logger.LogInformation("Response Generated");
            return new Response("SUCCESS", "Completed Handler Execution");
        }
    }
}
