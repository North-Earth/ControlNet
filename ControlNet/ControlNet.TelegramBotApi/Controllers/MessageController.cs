using System.Threading.Tasks;
using ControlNet.TelegramBotApi.Models;
using ControlNet.TelegramBotApi.Models.Services.BotService;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace ControlNet.TelegramBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        #region Fields

        private IBot Bot { get; }

        #endregion

        #region Constructors

        public MessageController(IBot bot)
        {
            Bot = bot;
        }

        #endregion

        #region Methods

        // GET: api/Message/[Token]
        [HttpGet]
        [Route(BotSettings.Token)]
        public IActionResult Get() => Ok();

        // POST: api/Message/[Token]
        /// <summary>
        /// Webhook sends messages for this bot using this method.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(BotSettings.Token)]
        public async Task Post([FromBody] Update update)
            => await Bot.MessageHandling(update);

        /// POST: api/Message/[Token]/SendMessage
        /// <summary>
        /// Telegram message sending.
        /// </summary>
        /// <param name="message">TelegramApi Message</param>
        /// <returns></returns>
        [HttpPost(BotSettings.Token + "/SendMessage")]
        public async Task SendMessage(Message message) 
            => await Bot.SendMessage(message);

        #endregion
    }
}
