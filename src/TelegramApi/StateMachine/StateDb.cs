using Exadel.OfficeBooking.TelegramApi.EF;
using Exadel.OfficeBooking.TelegramApi.Steps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public class StateDb
    {
        private readonly IConfiguration _config;
        private readonly TelegramDbContext _db;
        private readonly bool _isStoredInDb;
        private string _file = string.Empty;

        public StateDb(TelegramDbContext db, IConfiguration config)
        {
            _config = config;
            _db = db;
            _isStoredInDb = _config.GetValue<bool>("IsStoredInDb");
        }

        public async Task<UserState> GetState(long telegramId)
        {
            UserState? state;
            _file = telegramId.ToString() + ".json";
            if (_isStoredInDb)
            {
                state = await _db.UserStates.Include(f => f.User).AsNoTracking().FirstOrDefaultAsync(s => s.TelegramId == telegramId);
                if (state == null)
                {
                    state = new UserState() { TelegramId = telegramId, NextStep = nameof(Start) };
                    _db.UserStates.Add(state);
                    await _db.SaveChangesAsync();
                }
            }
            else
            {
                if (!System.IO.File.Exists(_file))
                {
                    System.IO.File.WriteAllText(_file, "");
                }

                state = JsonConvert.DeserializeObject<UserState>(System.IO.File.ReadAllText(_file)) ?? new UserState() { TelegramId = telegramId, NextStep = nameof(Start) };
            }

            return state;
        }

        public async Task SaveState(UserState state)
        {
            if (_isStoredInDb)
            {
                _db.UserStates.Update(state);
                await _db.SaveChangesAsync();
            }
            else
            {
                System.IO.File.WriteAllText(_file, JsonConvert.SerializeObject(state, Formatting.Indented));
            }
        }

        public async Task DeleteState(UserState state)
        {
            if (_isStoredInDb)
            {
                var userState = await _db.UserStates.AsNoTracking().FirstOrDefaultAsync(s => s.Id == state.Id);
                if (userState != null)
                {
                    _db.UserStates.Remove(userState);
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
