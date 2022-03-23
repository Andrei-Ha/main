using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.DTO.ParkingPlaceDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

/*
In this step, user can decide whether parking is necessary or not.
The previous step is DateChoice.
The next step is SpecParamChoice.
*/

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class ParkingChoice : StateMachineStep
    {
        private bool _hasParking;
        private bool _parkingAvailable;
        private ParkingPlaceGetDto _parking;
        private ParkingPlaceSetDto addParking;
        private readonly IHttpClientFactory _httpClient;

        public ParkingChoice(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<UserState> Execute(Update update)
        {
            var httpResponse = await _httpClient.GetWebApiModel<IEnumerable<OfficeGetDto>>("office", _state.User.Token);
            var httpResponseParking = await _httpClient.GetWebApiModel<IEnumerable<ParkingPlaceGetDto>>("parkingPlace", _state.User.Token);
            if (httpResponse.Model != null)
            {
                _hasParking = httpResponse.Model.First(p => p.Id == _state.OfficeId)
                   .IsFreeParkingAvailable;
            }
            if (httpResponseParking.Model != null)
            {
                var parkings = httpResponseParking.Model
                    .Where(p => p.OfficeId == _state.OfficeId)
                    .ToList();

                foreach(var parking in parkings)
                {
                    if(parking.IsBooked == false)
                    {
                        _parkingAvailable = true;
                        _parking = parking;
                        break;
                    }
                }
            }

            string? text = update.Message?.Text;
            if (_state.Propositions == null)
            {
                return _state;
            }

            // yes
            if (text == _state.Propositions[0])
            {
                if (_hasParking == false)
                {
                    _state.IsParkingPlace = false;
                    _state.TextMessage = "Office has no parking available, would You like to proceed without parking?";
                    _state.Propositions = new() { "Yes, proceed", "No" };
                    _state.NextStep = nameof(ParkingPlaceSpecifications);
                }
                else if (_parkingAvailable == false)
                {
                    _state.IsParkingPlace = false;
                    _state.TextMessage = "Office has no parking available, would You like to proceed without parking?";
                    _state.Propositions = new() { "Yes, proceed", "No" };
                    _state.NextStep = nameof(ParkingPlaceSpecifications);
                }

                else
                {
                    
                    _state.IsParkingPlace = true;
                    addParking = _parking;
                    addParking.IsBooked = true;
                    _state.ParkingPlace = _parking.PlaceNumber.ToString();
                    await _httpClient.PutWebApiModel<ParkingPlaceGetDto, ParkingPlaceSetDto>($"parkingPlace/put/{_parking.Id}", addParking, _state.User.Token);
                    _state.TextMessage = $"Parking place # {_parking.PlaceNumber} booked. Would you like to specify workplace parameters?";
                    _state.Propositions = new() { "Yes, I have special preferences", "No, I can take any available workplace" };
                    _state.NextStep = nameof(SpecParamChoice);
                   
                   
                }

            }
            
            // no
            else if (text == _state.Propositions[1])
            {
                _state.IsParkingPlace = false;
                _state.TextMessage = "Would you like to specify workplace parameters?";
                _state.Propositions = new() { "Yes, I have special preferences", "No, I can take any available workplace" };
                _state.NextStep = nameof(SpecParamChoice);
            }
            else
            {
                return _state;
            }

            return _state;
        }
    }
}