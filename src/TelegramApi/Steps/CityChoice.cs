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
    public class CityChoice : StateMachineStep
    {
        public CityChoice(IHttpClientFactory http) : base(http)
        {
        }

        public override async Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            var httpResponse = await GetModelFromWebAPI<IEnumerable<OfficeGetDto>>("office");
            if (httpResponse?.Model != null)
            {
                var propositions = httpResponse.Model
                    .Where(p => p.City == text)
                    .OrderBy(p => p.Name)
                    .Select(p => $"{p.Name} ({p.Address})")
                    .ToList();
                if (propositions != null)
                {
                    _fsmState.City = text!;
                    _fsmState.TextMessage = "Select the office:";
                    _fsmState.Propositions = propositions;
                    _fsmState.NextStep = nameof(OfficeChoice);
                }
            }

            return _fsmState;
        }
    }
}
