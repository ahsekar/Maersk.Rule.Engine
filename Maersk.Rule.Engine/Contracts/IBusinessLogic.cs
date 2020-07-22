using Microsoft.AspNetCore.Http;

namespace Maersk.Rule.Engine.Contracts
{
    public interface IBusinessLogic
    {
        bool ActivateMembership(HttpContext httpcontext);
        bool AddFirstAidVideo(HttpContext httpcontext);
        bool CreateDuplicateSlip(HttpContext httpcontext);
        bool NotifyEmail(HttpContext httpcontext);
        bool PhysicalProductPacking(HttpContext httpcontext);
        bool ProcessCommision(HttpContext httpcontext);
        bool UpgradeMembership(HttpContext httpcontext);
        //Add more business logics to extend this feature
    }
}
