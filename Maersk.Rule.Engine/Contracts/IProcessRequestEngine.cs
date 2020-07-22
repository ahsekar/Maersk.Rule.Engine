using Maersk.Promotion.Engine.Enums;
using Maersk.Rule.Engine.Models;

namespace Maersk.Rule.Engine.Contracts
{
    public interface IProcessRequestEngine
    {
        Response RunBusinessRuleEngine(PaymentTypes type);
    }
}