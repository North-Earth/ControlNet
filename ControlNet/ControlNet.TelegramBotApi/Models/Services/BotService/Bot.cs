using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ControlNet.TelegramBotApi.Models.Services.BotService
{
    public class Bot : IBot
    {
        #region Fields

        private readonly ITelegramBotClient _client;

        #endregion

        #region Constructors

        public Bot(ITelegramBotClient client)
        {
            _client = client;
        }

        #endregion

        #region Methods

        public async Task SetWebhookAsync(string webhookUrl)
        {
            await _client.SetWebhookAsync(webhookUrl);
        }

        public async Task MessageHandling(Update update)
        {
            await _client.SendTextMessageAsync(chatId: update.Message.Chat, text: "You said:\n" + update.Message.Text);
        }

        public async Task SendMessage(long chatId, string textMessage)
        {
            await _client.SendTextMessageAsync(chatId: chatId, text: textMessage);
        }

        public Task SendMessage(Message message)
        {
            //TODO: Create parse message.
            throw new System.NotImplementedException();
        }

        #endregion

    }
}
