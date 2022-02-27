using Exadel.OfficeBooking.TelegramApi.EF;
using Exadel.OfficeBooking.TelegramApi.FSM.Steps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM
{
    public class StateMachine
    {
        private IStep[] _steps;
        private UserState _userState = new();
        private readonly IServiceProvider _sp;
        private readonly TelegramDbContext _context;

        public StateMachine(IServiceProvider sp, TelegramDbContext context)
        {
            _sp = sp;
            _context = context;
            _steps = _sp.GetServices<IStep>().ToArray();
        }

        public async Task IncomingUpdateHandle(Update update)
        {
            await GetOrSetUserState(update);

            var step = _steps.First(step => step.GetType().Name == _userState.StepName);

            _userState = await step.CurrentStepHandle(update, _userState);

            await SaveUserState();
        }

        private async Task GetOrSetUserState(Update update)
        {
            _userState = await _context.UsersStates.AsNoTracking()
                .FirstOrDefaultAsync(u => u.ChatId == update.Message.Chat.Id);

            if (_userState == null)
            {
                _userState = new UserState { ChatId = update.Message.Chat.Id };

                await _context.UsersStates.AddAsync(_userState);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SaveUserState()
        {
            _context.UsersStates.Update(_userState);
            await _context.SaveChangesAsync();
        }
    }
}
