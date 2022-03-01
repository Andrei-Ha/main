﻿using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
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
        public Template(IHttpClientFactory http) : base(http)
        {
        }

        public override Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            throw new NotImplementedException();
        }
    }
}