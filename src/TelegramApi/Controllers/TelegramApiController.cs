using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Controllers
{
    [ApiController]
    [Route(template: "api/message")]
    public class TelController : ControllerBase
    {
        [HttpPost(template: "update")]
        public IActionResult Update(Update update)
        {
            return Ok();
        }
    }
}
