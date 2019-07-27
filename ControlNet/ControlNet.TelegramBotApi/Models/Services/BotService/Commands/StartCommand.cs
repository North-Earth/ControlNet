using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ControlNet.TelegramBotApi.Models.Services.BotService.Commands
{
    public class StartCommand : Command
    {
        #region Fields

        public override string Name => "/start";

        #endregion

        #region Methods

        public override Message Execute(string userName = null)
        {
            return new Message { Text = $"Welcome, {userName}!" };
        }

        #endregion
    }
}
