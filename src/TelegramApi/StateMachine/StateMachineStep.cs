using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public abstract class StateMachineStep
    {
        protected UserState _state = new();

        public void SetFsmState(UserState state)
        { 
            _state = state;
        }

        public abstract Task<UserState> Execute(Update update);
    }
}
