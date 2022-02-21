using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.States
{
    public interface IStateMachine
    {
        void SetState(StatesNamesEnum state);
        Task IncomingUpdateHandle(Update update);
    }
}
