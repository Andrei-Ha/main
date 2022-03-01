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
    public class OfficeChoise : StateMachineStep
    {
        public OfficeChoise(IHttpClientFactory http) : base(http)
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
                    _fsmState.Result.TextMessage = "Select booking type:";
                    _fsmState.Result.NextStep = nameof(DatesChoise);
                    _fsmState.Result.Propositions = new string[] {"One day", "Continuous"/*, "Recurring"*/};
                }
            }
            return _fsmState;
        }
    }
}