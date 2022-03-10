using Exadel.OfficeBooking.TelegramApi.DTO;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public class StateMachine
    {
        private readonly StateMachineStep[] _steps = Array.Empty<StateMachineStep>();
        private UserState _state = new();
        private readonly StateDb _db;

        public StateMachine(IServiceProvider serviceProvider, StateDb db)
        {
            _steps = serviceProvider.GetServices<StateMachineStep>().ToArray();
            _db = db;
        }

        public async Task GetState(long telegramId)
        {
            _state = await _db.GetState(telegramId);
        }

        public async Task<Result> Process(Update update)
        {
            var curStep = GetCurrentStep();
            curStep.SetFsmState(_state);
            _state = await curStep.Execute(update);
            if (_state.NextStep != "Finish")
            {
                await SaveState();
            }
            else
            {
                await DeleteState();
            }

            return _state.GetResult();
        }

        private StateMachineStep GetCurrentStep()
        {
            return _steps.First(s => s.GetType().Name == _state.NextStep);
        }

        private async Task SaveState()
        {
            await _db.SaveState(_state);
        }

        private async Task DeleteState()
        {
            await _db.DeleteState(_state);
        }
    }
}
