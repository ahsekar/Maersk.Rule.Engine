using Maersk.Promotion.Engine.Abstractions;
using Maersk.Rule.Engine.Models;
using Microsoft.AspNetCore.Http;

namespace Maersk.Rule.Engine.Contracts
{
    public interface IProcessHandler
    {
        Response Invoke(HttpContext httpContext);
        void Next(HandlerDelegate next);
    }
}