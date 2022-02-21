using Exadel.OfficeBooking.TelegramApi.States;
using Exadel.OfficeBooking.TelegramApi.States.StatesHandlers;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi
{
    public class StateMachine : IStateMachine
    {
        private StatesNamesEnum _state;

        private StatesNamesEnum _previousState;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TelegramBotClient _botClient;

        public StateMachine(TelegramBot telegramBot, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _botClient = telegramBot.GetBot().Result;
        }

        public async Task IncomingUpdateHandle(Update update)
        {
            switch (_state)
            {
                case StatesNamesEnum.Greetings:
                    var greetingsStateHandler = new GreetingsStateHandler(_botClient, _httpClientFactory);
                    SetState(await greetingsStateHandler.CurrentStateHandle(update));
                    break;

                case StatesNamesEnum.SelectCity:
                    var cityStateHandler = new SelectCityStateHandler(_botClient, _httpClientFactory);
                    SetState(await cityStateHandler.CurrentStateHandle(update));
                    break;

                case StatesNamesEnum.SelectOffice:
                    // state SelectOffice logic
                    break;

                case StatesNamesEnum.SelectBookingType:
                    // state SelectBookingType logic
                    break;

                default:
                    break;
            }
        }

        public void SetState(StatesNamesEnum state)
        {
            //switch (_state)
            //{
            //    case StatesNamesEnum.SelectCity:
            //        // state SelectCity common exit logic
            //        break;

            //    case StatesNamesEnum.SelectOffice:
            //        // state SelectOffice common exit logic
            //        break;

            //    case StatesNamesEnum.SelectBookingType:
            //        // state SelectBookingType common exit logic
            //        break;

            //    default:
            //        break;
            //}

            _previousState = _state;

            _state = state;
        }
    }
}
