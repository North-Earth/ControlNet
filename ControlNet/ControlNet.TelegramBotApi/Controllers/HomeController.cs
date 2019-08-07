using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ControlNet.TelegramBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        #region Fields

        #endregion

        #region Constructors

        public HomeController() { }

        #endregion

        #region Methods

        [HttpGet]
        public string Get()
        {
            return "Hello! I'm Telegergam Bot API. :)";
        }

        #endregion
    }
}
