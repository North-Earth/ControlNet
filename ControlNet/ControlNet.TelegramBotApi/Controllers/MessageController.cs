using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControlNet.TelegramBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        #region Fields

        #endregion

        #region Constructors

        public MessageController()
        {

        }

        #endregion

        #region Methods

        // GET: api/Message
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        #endregion
    }
}
