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
    public class OfficeChoice : StateMachineStep
    {
        public OfficeChoice(IHttpClientFactory http) : base(http)
        {
        }

        public override async Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            var httpResponse = await GetModelFromWebAPI<IEnumerable<OfficeGetDto>>("office");
            if (httpResponse?.Model != null)
            {
                var office = httpResponse.Model.FirstOrDefault(p => $"{p.Name} ({p.Address})" == text);
                if (office != null)
                {
                    _fsmState.OfficeId= office.Id;
                    _fsmState.OfficeName = text!;
                    _fsmState.TextMessage = "Select booking type:";
                    _fsmState.Propositions = new() {"One day", "Continuous", "Recurring"};
                    _fsmState.NextStep = nameof(DatesChoice);
                }
            }
            return _fsmState;
        }
    }
}