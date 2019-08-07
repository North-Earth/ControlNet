using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlNet.TelegramBotApi.Models.Services.BotService.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ControlNet.TelegramBotApi.Models.Services.BotService
{
    public class Bot : IBot
    {
        #region Fields

        private ITelegramBotClient Client { get; }

        private IEnumerable<Command> Commands { get; }

        #endregion

        #region Constructors

        public Bot(ITelegramBotClient client)
        {
            Client = client;

            Commands = new List<Command>
            {
                new StartCommand()
                //TODO: Next commands.
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// WebHook installation for the bot.
        /// </summary>
        /// <param name="webhookUrl"></param>
        /// <returns></returns>
        public async Task SetWebhookAsync(string webhookUrl)
        {
            await Client.SetWebhookAsync(webhookUrl);
        }

        /// <summary>
        /// Processing received messages and commands using a WebHook.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task MessageHandling(Update update)
        {
            var message = update.Message;
            var command = GetCommands().Where(cmd => cmd.Name == message.Text)?.FirstOrDefault();

            var replyMessage = new Message { Text = "🤔" };

            if (command != null)
                replyMessage = command.Execute(message.Chat.FirstName);


            //TODO: Use other overload SendMessage.
            await SendMessage(chatId: message.Chat.Id, textMessage: replyMessage.Text, replyToMessageId: message.MessageId);

            //await SendMessage(replyMessage);
        }

        public async Task SendMessage(long chatId, string textMessage)
            => await Client.SendTextMessageAsync(chatId: chatId, text: textMessage);

        public async Task SendMessage(long chatId, string textMessage, int replyToMessageId)
            => await Client.SendTextMessageAsync(chatId: chatId, text: textMessage, replyToMessageId: replyToMessageId);

        public Task SendMessage(Message message)
        {
            //TODO: Create parse message.
            throw new System.NotImplementedException();
        }

        public IEnumerable<Command> GetCommands() => Commands;

        #endregion

    }
}
