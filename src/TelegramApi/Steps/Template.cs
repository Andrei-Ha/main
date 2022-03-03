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
    public class Template : StateMachineStep
    {
        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            _state.SetResult();
            return _state;
        }
    }
}