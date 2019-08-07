using Telegram.Bot.Types;

namespace ControlNet.TelegramBotApi.Models.Services.BotService.Commands
{
    public abstract class Command
    {
        #region Fields

        public abstract string Name { get; }

        #endregion

        #region Methods

        public abstract Message Execute(string userName = null);

        #endregion
    }
}
