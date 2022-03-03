using Exadel.OfficeBooking.TelegramApi.EF;
using Exadel.OfficeBooking.TelegramApi.Steps;
using Microsoft.EntityFrameworkCore;
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
        private long _telegramId = 0;
        private string _file = string.Empty;
        private FsmState _state = new();
        private readonly TelegramDbContext _db;
        private bool _isStoredInDb;

        public StateMachine(IServiceProvider serviceProvider, TelegramDbContext db)
        {
            _steps = serviceProvider.GetServices<StateMachineStep>().ToArray();
            _db = db;
        }

        public async Task Init(long telegramId, bool isStoredInDb = true)
        {
            _telegramId = telegramId;
            _file = telegramId.ToString() + ".json";
            _isStoredInDb = isStoredInDb;
            if (_isStoredInDb)
            {
                var fsmState = await _db.FsmStates.Include(f => f.User).AsNoTracking().FirstOrDefaultAsync(s => s.TelegramId == telegramId);
                if (fsmState != null)
                {
                    _state = fsmState;
                }
                else
                {
                    _state = new FsmState() { TelegramId = telegramId, NextStep = nameof(Start) };
                    _db.FsmStates.Add(_state);
                    await _db.SaveChangesAsync();
                }
            }
            else
            {
                if (!System.IO.File.Exists(_file))
                {
                    System.IO.File.WriteAllText(_file, "");
                }

                _state = JsonConvert.DeserializeObject<FsmState>(System.IO.File.ReadAllText(_file)) ?? new FsmState() { TelegramId = telegramId, NextStep = nameof(Start) };
            }
        }

        public async Task<Result> Process(Update update)
        {
            var curStep = GetCurrentStep();
            curStep.TransmitFsmState(_state);
            _state = await curStep.Execute(update);
            if (_state.NextStep != "Finish")
            {
                await SaveState();
            }
            else
            {
                await DeleteFileState();
            }

            return _state.GetResult();
        }

        private StateMachineStep GetCurrentStep()
        {
            return _steps.First(s => s.GetType().Name == _state.NextStep);
        }

        private async Task SaveState()
        {
            if (_isStoredInDb)
            {
                _db.FsmStates.Update(_state);
                await _db.SaveChangesAsync();
            }
            else
            {
                System.IO.File.WriteAllText(_file, JsonConvert.SerializeObject(_state, Formatting.Indented));
            }
        }

        private async Task DeleteFileState()
        {
            if (_isStoredInDb)
            {
                var fsmState = await _db.FsmStates.AsNoTracking().FirstOrDefaultAsync(s => s.Id == _state.Id);
                if (fsmState != null)
                {
                    _db.FsmStates.Remove(_state);
                    await _db.SaveChangesAsync();
                }
            }
            else
            {
                if (System.IO.File.Exists(_file))
                {
                    System.IO.File.Delete(_file);
                }
            }
        }
    }
}
