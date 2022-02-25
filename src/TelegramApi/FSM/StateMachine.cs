using Exadel.OfficeBooking.TelegramApi.FSM;
using Exadel.OfficeBooking.TelegramApi.FSM.Steps;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM
{
    public class StateMachine
    {
        private IStep[] _steps;
        private FsmState _state = new();
        private readonly IServiceProvider _sp;

        public StateMachine(IServiceProvider sp)
        {
            _sp = sp;
            _steps = _sp.GetServices<IStep>().ToArray();
        }

        public async Task IncomingUpdateHandle(Update update)
        {
            // add FsmState to TelegramApi database if not exist
            // if FsmState exist in database for incoming user, get current StepName

            var step = _steps.First(step => step.GetType().Name == _state.StepName.ToString());

            _state = await step.CurrentStepHandle(update);
        }
    }
}
