using Exadel.OfficeBooking.TelegramApi.FSM.Steps;
using Microsoft.Extensions.DependencyInjection;

namespace Exadel.OfficeBooking.TelegramApi.FSM
{
    public static class FsmServicesExtension
    {
        public static void FsmServices(this IServiceCollection services)
        {
            services.AddScoped<StateMachine>();
            
            services.AddScoped<IStep, Greetings>();

            services.AddScoped<IStep, SelectCity>();
        }
    }
}
