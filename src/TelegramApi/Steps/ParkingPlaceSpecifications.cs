using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class ParkingPlaceSpecifications : StateMachineStep
    {
        private readonly IHttpClientFactory _httpClient;
        private bool hasParking;

        public ParkingPlaceSpecifications(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;

            if (_state.Propositions == null)
            {
                return _state;
            }

            if (text == _state.Propositions[0])
            {
                _state.IsParkingPlace = false;
                _state.TextMessage = "Would you like to specify workplace parameters?";
                _state.Propositions = new() { "Yes, I have special preferences", "No, I can take any available workplace" };
                _state.NextStep = nameof(SpecParamChoice); 
            }

            else if (text == _state.Propositions[1])
            {
                _state.SetResult(textMessage: "Bye! See you later");
            }
           

            return _state;
        }
    }
}

