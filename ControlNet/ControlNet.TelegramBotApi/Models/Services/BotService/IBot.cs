using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ControlNet.TelegramBotApi.Models.Services.BotService
{
    public interface IBot
    {
        #region Methods

        Task SetWebhookAsync(string webhookUrl);

        Task MessageHandling(Update update);

        Task SendMessage(long chatId, string textMessage);

        Task SendMessage(Message message);

        #endregion
    }
}
