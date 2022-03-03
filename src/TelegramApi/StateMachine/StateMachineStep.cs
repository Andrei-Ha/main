using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public abstract class StateMachineStep
    {
        protected FsmState _fsmState = new();

        public void TransmitFsmState(FsmState fsmState)
        { 
            _fsmState = fsmState;
        }

        public abstract Task<FsmState> Execute(Update update);
    }
}
