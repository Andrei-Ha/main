using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Exadel.OfficeBooking.TelegramApi
{
	public class TelegramBot
	{
		private readonly IConfiguration _configuration;
		private TelegramBotClient? _botClient;

		public TelegramBot(IConfiguration configuration)
		{
			_configuration = configuration;
		}

        public async Task<TelegramBotClient> GetBot()
		{
			if (_botClient != null)
			{
				return _botClient;
			}

			_botClient = new TelegramBotClient(_configuration["Token"]);

			await _botClient.SetWebhookAsync($"{_configuration["Url"]}api/message/update");

			return _botClient;
		}
	}
}
