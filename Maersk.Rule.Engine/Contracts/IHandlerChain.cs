using Maersk.Promotion.Engine.Enums;
using Maersk.Rule.Engine.Contracts;

namespace Maersk.Rule.Engine.Handlers
{
    public interface IHandlerChain
    {
        IProcessHandler RegisterOutBoundHandlers(PaymentTypes type);
    }
}