using ControlNet.MonitoringService.Helpers;
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
            => Configuration = configuration;

        #endregion

        #region Methods

        public async Task StartServiceAsync()
        {
            var batteryStatus = MachineInformationService.GetBatteryStatus() == "1" ? "Doesn't charge." : "Charging.";

            var message = $"{ MachineInformationService.GetMachineName() } Online. \n" +
                $"Services started successfully. " +
                $"[{DateTime.Now}] \n" +
                $"Battery Charge: {MachineInformationService.GetBatteryCharge()}% \n" +
                $"Status: {batteryStatus}";

            await SendReportAsync(message);

            await Task.Factory.StartNew(async () => await MonitoringBatteryCharge());
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

        public async Task MonitoringBatteryCharge()
        {
            int warningLevel = 0;
            int delayNotification = 7; // Minutes.

            DateTime? lastWarningTime = DateTime.Now.AddMinutes(0 - delayNotification);

            string message = string.Empty;

            while (true)
            {
                var batteryStatus = int.Parse(MachineInformationService.GetBatteryStatus());
                var batteryCharge = int.Parse(MachineInformationService.GetBatteryCharge());

                // Battery doesn't charge.
                if (batteryStatus == 1)
                {
                    if (FunctionsHelper.Between(
                        num: batteryCharge, lower: 11, upper: 25, inclusive: true))
                    {
                        message = $"Warning[{MachineInformationService.GetMachineName()}]! \n" +
                            $"Low battery({batteryCharge}%).";

                        warningLevel = 1;
                    }
                    else if (batteryCharge <= 10)
                    {
                        message = $"Alarm[{MachineInformationService.GetMachineName()}]! \n" +
                            $"Battery level critical ({batteryCharge}%)!";

                        warningLevel = 2;
                    }
                }
                else
                {
                    if (batteryCharge == 100)
                    {
                        message = $"Information [{MachineInformationService.GetMachineName()}]. \n" +
                            $"Battery fully charged. ({batteryCharge}%). \n" +
                            $"Unplug the charger!";

                        warningLevel = 0;
                    }
                    else if (batteryCharge >= 95)
                    {
                        message = $"Information [{MachineInformationService.GetMachineName()}]. \n" +
                            $"Battery is almost charged. ({batteryCharge}%). \n" +
                            $"Recommendation to turn off the charger.";

                        warningLevel = 0;
                    }
                }

                switch (warningLevel)
                {
                    case 0:
                        delayNotification = 7;
                        break;
                    case 1:
                        delayNotification = 3;
                        break;
                    case 2:
                        delayNotification = 1;
                        break;
                }

                var notificationtDataTime = DateTime.Now.AddMinutes(0 - delayNotification);

                if (notificationtDataTime >= lastWarningTime && !string.IsNullOrEmpty(message))
                {
                    await SendReportAsync(message);
                    lastWarningTime = DateTime.Now;
                    message = string.Empty;
                }

                // 3 minutes.
                await Task.Delay(30000);
            }
        }

        #endregion
    }
}
