using Exadel.OfficeBooking.TelegramApi.EF;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public class SelectOffice : IStep
    {
        public Task<UserState> CurrentStepHandle(Update update, UserState state)
        {
            throw new System.NotImplementedException();
        }
    }
}
