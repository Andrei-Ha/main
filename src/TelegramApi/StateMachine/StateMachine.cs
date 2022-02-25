﻿using Exadel.OfficeBooking.TelegramApi.Steps;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Telegram.Bot.Types;
using static Exadel.OfficeBooking.TelegramApi.StateMachine.StateMachineStep;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public class StateMachine
    {
        private StateMachineStep[] _steps = { };
        private string _file = string.Empty;
        private FsmState _state = new();

        public StateMachine(IServiceProvider serviceProvider)
        {
            // Initial all steps
            _steps = serviceProvider.GetServices<StateMachineStep>()
                .Select(o => 
                {
                    o.State = new();
                    return o;
                }
            ).ToArray();
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

        public Result Process(Update update)
        {
            var curStep = GetCurrentStep();
            var newState = curStep.Execute(update,  _state);
            _state.StepName = newState.Result.NextStep;
            SaveState();
            return newState.Result;
        }

        private StateMachineStep GetCurrentStep()
        {
            return _steps.First(s => s.GetType().Name == _state.StepName);
        }

        private void SaveState()
        {
            System.IO.File.WriteAllText(_file, JsonConvert.SerializeObject(_state, Formatting.Indented));
        }
    }
}
