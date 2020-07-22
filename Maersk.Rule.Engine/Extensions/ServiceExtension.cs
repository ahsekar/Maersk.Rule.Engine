using Maersk.Rule.Engine.Business;
using Maersk.Rule.Engine.Contracts;
using Maersk.Rule.Engine.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Maersk.Rule.Engine.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// This extension method can be added in the Startup class of consuming application which will register all services required for this library
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RuleServices(this IServiceCollection services)
        {
            services.AddTransient<Func<string, IProcessHandler>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "AddFirstAidVideoHandler":
                        return serviceProvider.GetService<AddFirstAidVideoHandler>();
                    case "ProcessCommissionPaymentHandler":
                        return serviceProvider.GetService<ProcessCommissionPaymentHandler>();
                    case "PhysicalProductPackingSlipHandler":
                        return serviceProvider.GetService<PhysicalProductPackingSlipHandler>();
                    case "CreateDuplicateSlipForRoyaldepartmentHandler":
                        return serviceProvider.GetService<CreateDuplicateSlipForRoyaldepartmentHandler>();
                    case "NotifyEmailHandler":
                        return serviceProvider.GetService<NotifyEmailHandler>();
                    case "ActivateMemeberShipHandler":
                        return serviceProvider.GetService<ActivateMemeberShipHandler>();
                    case "UpgradeMembershipHandler":
                        return serviceProvider.GetService<UpgradeMembershipHandler>();
                    default:
                        return null;
                }
            });

            services.AddSingleton<IBusinessLogic, BusinessLogic>();
            services.AddSingleton<IProcessRequestEngine, ProcessRequestEngine>();
            services.AddSingleton<IHandlerChain, HandlerChain>();

            return services;
        }
    }
}
