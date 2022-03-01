using Exadel.OfficeBooking.TelegramApi.Steps;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public class StateMachine
    {
        private readonly StateMachineStep[] _steps = Array.Empty<StateMachineStep>();
        private string _file = string.Empty;
        private FsmState _state = new();

        public StateMachine(IServiceProvider serviceProvider)
        {
            _steps = serviceProvider.GetServices<StateMachineStep>().ToArray();
        }

        public void Init(long telegramId)
        {
            _file = telegramId.ToString() + ".json";
            if (!System.IO.File.Exists(_file))
            {
                System.IO.File.WriteAllText(_file, "");
            }

            _state = JsonConvert.DeserializeObject<FsmState>(System.IO.File.ReadAllText(_file)) ?? new FsmState() { TelegramId = telegramId, StepName = nameof(Start) };
        }

        public async Task<Result> Process(Update update)
        {
            var curStep = GetCurrentStep();
            curStep.TransmitFsmState(_state);
            _state = await curStep.Execute(update);
            _state.StepName = _state.Result.NextStep;
            if (_state.StepName != "Finish")
            {
                SaveState();
            }
            else
            {
                DeleteFileState();
            }

            return _state.Result;
        }

        private StateMachineStep GetCurrentStep()
        {
            return _steps.First(s => s.GetType().Name == _state.StepName);
        }

        private void SaveState()
        {
            System.IO.File.WriteAllText(_file, JsonConvert.SerializeObject(_state, Formatting.Indented));
        }

        private void DeleteFileState()
        {
            if (System.IO.File.Exists(_file))
            {
                System.IO.File.Delete(_file);
            }
        }
    }
}
