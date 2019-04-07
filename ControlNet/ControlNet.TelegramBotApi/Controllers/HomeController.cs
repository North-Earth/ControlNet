using Microsoft.AspNetCore.Mvc;

namespace ControlNet.TelegramBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        #region Methods

        [HttpGet]
        public string Get() => "Hello! I'm Telegergam Bot API. :)";

        #endregion
    }
}
