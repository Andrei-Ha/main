using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.FSM.Steps;
using Microsoft.Extensions.DependencyInjection;

namespace Exadel.OfficeBooking.TelegramApi.FSM
{
    public static class FsmServicesExtension
    {
        public static void FsmServices(this IServiceCollection services)
        {
            services.AddScoped<StateMachine>();
            
            services.AddScoped<Step, Greetings>();
            
            services.AddScoped<Step, ActionChoice>();

            services.AddScoped<Step, SelectCity>();

            services.AddScoped<Step, SelectOffice>();

            services.AddScoped<Step, Finish>();
        }
    }
}
