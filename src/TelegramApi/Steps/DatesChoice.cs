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
        public DatesChoice(IHttpClientFactory http) : base(http)
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
                        if (_fsmState.Propositions == null)
                        {
                            return _fsmState;
                        }

                        // One day
                        if (text == _fsmState.Propositions[0])
                        {
                            _fsmState.BookingType = BookingTypeEnum.OneDay;
                            start = string.Empty;
                        }
                        // Continuous
                        else if (text == _fsmState.Propositions[1])
                        {
                            _fsmState.BookingType = BookingTypeEnum.Continuous;
                        }
                        // Reccuring
                        else if (text == _fsmState.Propositions[2])
                        {
                            _fsmState.BookingType = BookingTypeEnum.Recurring;
                        }
                        _fsmState.TextMessage = $"Enter the {start} date in the format dd.mm.yyyy";
                        _fsmState.Propositions = default;
                        // fsmState.NextState does not change
                        break;
                    }

                case BookingTypeEnum.OneDay:
                    {
                        if (DateTime.TryParse(text, out DateTime dateStart))
                        {
                            _fsmState.DateStart = dateStart;
                            _fsmState.TextMessage = "Would you like to add parking place?";
                            _fsmState.Propositions = new() { "yes", "no" };
                            _fsmState.NextStep = nameof(ParkingChoice);
                        }
                        break;
                    }
                case BookingTypeEnum.Continuous:
                    {
                        if (_fsmState.DateStart == default(DateTime) && DateTime.TryParse(text, out DateTime dateStart)) 
                        {
                            _fsmState.DateStart = dateStart;
                            _fsmState.TextMessage = "Enter the End date in the format dd.mm.yyyy";
                            // fsmState.NextState does not change and proposition = default
                        }
                        else if(_fsmState.DateEnd == default && DateTime.TryParse(text, out DateTime dateEnd))
                        {
                            _fsmState.DateEnd = dateEnd;
                            _fsmState.TextMessage = "Would you like to add parking place?";
                            _fsmState.Propositions = new() { "yes", "no" };
                            _fsmState.NextStep = nameof(ParkingChoice);
                        }
                        break;
                    }
                case BookingTypeEnum.Recurring:
                    {
                        _fsmState.SetResult();
                        break;
                    }

            }
            return _fsmState;
        }
    }
}