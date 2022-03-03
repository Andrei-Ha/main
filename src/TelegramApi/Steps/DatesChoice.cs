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
    public class DatesChoice : StateMachineStep
    {
        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            string start = "Start";

            switch (_state.BookingType)
            {
                case BookingTypeEnum.None:
                    {
                        if (_state.Propositions == null)
                        {
                            return _state;
                        }

                        // One day
                        if (text == _state.Propositions[0])
                        {
                            _state.BookingType = BookingTypeEnum.OneDay;
                            start = string.Empty;
                        }
                        // Continuous
                        else if (text == _state.Propositions[1])
                        {
                            _state.BookingType = BookingTypeEnum.Continuous;
                        }
                        // Reccuring
                        else if (text == _state.Propositions[2])
                        {
                            _state.BookingType = BookingTypeEnum.Recurring;
                        }
                        _state.TextMessage = $"Enter the {start} date in the format dd.mm.yyyy";
                        _state.Propositions = default;
                        // state.NextState does not change
                        break;
                    }

                case BookingTypeEnum.OneDay:
                    {
                        if (DateTime.TryParse(text, out DateTime dateStart))
                        {
                            _state.DateStart = dateStart;
                            _state.TextMessage = "Would you like to add parking place?";
                            _state.Propositions = new() { "yes", "no" };
                            _state.NextStep = nameof(ParkingChoice);
                        }
                        break;
                    }
                case BookingTypeEnum.Continuous:
                    {
                        if (_state.DateStart == default(DateTime) && DateTime.TryParse(text, out DateTime dateStart)) 
                        {
                            _state.DateStart = dateStart;
                            _state.TextMessage = "Enter the End date in the format dd.mm.yyyy";
                            // state.NextState does not change and proposition = default
                        }
                        else if(_state.DateEnd == default && DateTime.TryParse(text, out DateTime dateEnd))
                        {
                            _state.DateEnd = dateEnd;
                            _state.TextMessage = "Would you like to add parking place?";
                            _state.Propositions = new() { "yes", "no" };
                            _state.NextStep = nameof(ParkingChoice);
                        }
                        break;
                    }
                case BookingTypeEnum.Recurring:
                    {
                        _state.SetResult();
                        break;
                    }

            }
            return _state;
        }
    }
}