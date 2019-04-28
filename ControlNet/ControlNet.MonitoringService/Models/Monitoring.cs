using ControlNet.MonitoringService.Models.Sevices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ControlNet.MonitoringService.Models
{
    public class Monitoring : IMonitoring
    {
        #region Fields

        public IConfiguration Configuration { get; }

        #endregion

        #region Constructors

        public Monitoring(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Methods

        public async Task StartServiceAsync()
        {
            var message = $"{ MachineInformationService.GetMachineName() } Online. \n" +
                $"Services started successfully. \n" +
                $"[{DateTime.Now}]";

            await SendReportAsync(message);
        }

        public async Task SendReportAsync(string message)
        {
            var chatId = Configuration.GetValue<int>("TelegramBot:MainReportChatId");
            var botApiUrl = Configuration.GetValue<string>("ServiceUrl:TelegramBotApi");

            var messageRoute = Configuration.GetValue<string>("RouteSettings:SendMessage")
                .Replace("{token}", AppSettings.Token);

            var data = new Message { Chat = new Chat { Id = chatId }, Text = message, Date = DateTime.Now };

            await RestClientService<Message>
                .SendPostRequest(botApiUrl, route: messageRoute, data);
        }

        #endregion
    }
}
