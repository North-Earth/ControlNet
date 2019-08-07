using System.Collections.Generic;
using System.Threading.Tasks;
using ControlNet.TelegramBotApi.Models.Services.BotService.Commands;
using Telegram.Bot.Types;

namespace ControlNet.TelegramBotApi.Models.Services.BotService
{
    public interface IBot
    {
        #region Methods

        Task SetWebhookAsync(string webhookUrl);

        Task MessageHandling(Update update);

        Task SendMessage(long chatId, string textMessage);

        Task SendMessage(long chatId, string textMessage, int replyToMessageId);

        Task SendMessage(Message message);

        IEnumerable<Command> GetCommands();

        #endregion
    }
}
