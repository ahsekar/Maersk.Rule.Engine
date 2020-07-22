using Maersk.Rule.Engine.Models;
using Microsoft.AspNetCore.Http;

namespace Maersk.Promotion.Engine.Abstractions
{
    public delegate Response HandlerDelegate(HttpContext httpContext);
}


