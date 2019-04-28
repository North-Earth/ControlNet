using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlNet.TelegramBotApi.Models;
using ControlNet.TelegramBotApi.Models.Services.BotService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace ControlNet.TelegramBotApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        #region Fields

        private readonly IBot _bot;

        #endregion

        #region Constructors

        public MessageController(IBot bot)
            => _bot = bot;

        #endregion

        #region Methods

        // GET: api/Message/[Token]
        [HttpGet]
        [Route(BotSettings.Token)]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        // POST: api/Message/[Token]
        [HttpPost]
        [Route(BotSettings.Token)]
        public async Task Post([FromBody] Update update)
            => await _bot.MessageHandling(update);

        [HttpPost(BotSettings.Token + "/SendMessage")]
        public async Task SendMessage(Message message)
        {
            var chatId = message.Chat.Id;
            var textMessage = message.Text;

            await _bot.SendMessage(chatId, textMessage);
        }

        #endregion
    }
}
