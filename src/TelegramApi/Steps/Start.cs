using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Linq;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class Start : StateMachineStep
    {
        private readonly IHttpClientFactory _httpClient;
        public Start(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public override async Task<UserState> Execute(Update update)
        {
            var httpResponse = await _httpClient.PostWebApiModel<LoginUserDto, string>("login", _state.TelegramId.ToString());
            var user = httpResponse?.Model;
            if ( user != null)
            {
                _state.User = user;
                if (_state.User.Role == UserRole.Admin || _state.User.Role == UserRole.Manager)
                {
                    _state.TextMessage = $"Hello, {_state.User.FirstName}! You have {user.Role} rights.\n Who would you like to manage workplaces for?";
                    _state.Propositions = new() { "myself", "other employee" };
                    _state.NextStep = nameof(ManageForChoice);
                }
                else
                {
                    _state.TextMessage = $"Hello, {_state.User.FirstName}! What do you want to do today?";
                    _state.Propositions = new() { "Change or Cancel a booking", "Book a workplace", "Nothing" };
                    _state.NextStep = nameof(ActionChoice);
                }
            }
            else
            {
                _state.SetResult(textMessage: "Sorry, you can't do booking!");
                // or
                //_state.TextMessage = "Sorry, you can't do booking!";
                //_state.Propositions = default;
                //_state.NextStep = "Finish";
            }

            return _state;
        }
    }
}
