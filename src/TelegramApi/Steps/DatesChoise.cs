using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class DatesChoise : StateMachineStep
    {
        public DatesChoise(IHttpClientFactory http) : base(http)
        {
        }

        public override async Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            string start = "Start";
            switch (_fsmState.BookingType)
            {
                case BookingTypeEnum.None:
                    {
                        // One day
                        if (text == _fsmState.Result.Propositions[0])
                        {
                            _fsmState.BookingType = BookingTypeEnum.OneDay;
                            start = string.Empty;
                        }
                        // Continuous
                        else if (text == _fsmState.Result.Propositions[1])
                        {
                            _fsmState.BookingType = BookingTypeEnum.Continuous;
                        }
                        // Reccuring
                        else if (text == _fsmState.Result.Propositions[2])
                        {
                            _fsmState.BookingType = BookingTypeEnum.Recurring;
                        }
                        _fsmState.Result.TextMessage = $"Enter the {start} date in the format dd.mm.yyyy";
                        _fsmState.Result.Propositions = new();
                        break;
                    }

                case BookingTypeEnum.OneDay:
                    {
                        if (DateTime.TryParse(text, out DateTime dateStart))
                        {
                            _fsmState.DateStart = dateStart;
                            _fsmState.Result.TextMessage = "Would you like to add parking place?";
                            _fsmState.Result.NextStep = nameof(ParkingChoise);
                            _fsmState.Result.Propositions = new() { "yes", "no" };
                        }
                        break;
                    }
                case BookingTypeEnum.Continuous:
                    {
                        if (_fsmState.DateStart == default(DateTime) && DateTime.TryParse(text, out DateTime dateStart)) 
                        {
                            _fsmState.DateStart = dateStart;
                            _fsmState.Result.TextMessage = "Enter the End date in the format dd.mm.yyyy";
                        }
                        else if(_fsmState.DateEnd == default(DateTime) && DateTime.TryParse(text, out DateTime dateEnd))
                        {
                            _fsmState.DateEnd = dateEnd;
                            _fsmState.Result.TextMessage = "Would you like to add parking place?";
                            _fsmState.Result.NextStep = nameof(ParkingChoise);
                            _fsmState.Result.Propositions = new() { "yes", "no" };
                        }
                        break;
                    }
                case BookingTypeEnum.Recurring:
                    {

                        break;
                    }

            }
            return _fsmState;
        }
    }
}