using ControlNet.Logger;
using ControlNet.MonitoringService.Helpers;
using ControlNet.MonitoringService.Models.Sevices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ControlNet.MonitoringService.Models
{
    public class Monitoring : IMonitoring
    {
        #region Fields

        private IConfiguration Configuration { get; }

        private ILogger Logger { get; }

        private IResourcesService ResourcesService { get; }

        #endregion

        #region Constructors

        public Monitoring(IConfiguration configuration, ILogger logger, IResourcesService resourcesService)
        {
            Configuration = configuration;
            Logger = logger;
            ResourcesService = resourcesService;
        }

        #endregion

        #region Methods

        public async Task StartServiceAsync()
        {
            var message = $"{ MachineInformationService.GetMachineName() } Online. \n" +
                $"Services started successfully. " +
                $"[{DateTime.Now}] \n" +
                $"Battery Charge: {MachineInformationService.GetBatteryCharge()}% \n" +
                $"Charging Status: {MachineInformationService.IsCharging}";

            await SendReportAsync(message);

            try
            {
                var tasks = new List<Task>()
                {
                    await Task.Factory.StartNew(async () => await BatteryChargeMonitoring(), TaskCreationOptions.LongRunning),
                    await Task.Factory.StartNew(async () => await WorkTimeMonitoring(), TaskCreationOptions.LongRunning)
                };

                Task.WaitAll(tasks.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            await Logger.WriteInformationAsync("Send message:\n" + message);
        }

        public async Task BatteryChargeMonitoring()
        {
            await Logger.WriteInformationAsync("BatteryChargeMonitoring is running.");
            int warningLevel = 0;
            int delayNotificationMinutes = 7;

            DateTime? lastWarningTime = DateTime.Now.AddMinutes(0 - delayNotificationMinutes);

            string message = string.Empty;

            while (true)
            {
                var batteryCharge = int.Parse(MachineInformationService.GetBatteryCharge());

                // Battery doesn't charge.
                if (!MachineInformationService.IsCharging)
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
                        delayNotificationMinutes = 7;
                        break;
                    case 1:
                        delayNotificationMinutes = 3;
                        break;
                    case 2:
                        delayNotificationMinutes = 1;
                        break;
                }

                var notificationtDataTime = DateTime.Now.AddMinutes(0 - delayNotificationMinutes);

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

        public async Task WorkTimeMonitoring()
        {
            await Logger.WriteInformationAsync("WorkTimeMonitoring is running.");

            string workTimeKey = "WorkTime";
            string lastWorkTimeKey = "LastWorkTime";

            DateTime currentDateTime = DateTime.Now;
            var emptyTimeSpan = new TimeSpan().ToString();

            while (true)
            {
                var workTime = new TimeSpan();

                try
                {
                    workTime = TimeSpan.Parse(ResourcesService.GetResource(workTimeKey));
                }
                catch (ArgumentNullException ex)
                {
                    await Logger.WriteErrorAsync("WorkTimeMonitoring: Resource file is not found.");
                    await Logger.WriteWarningAsync("WorkTimeMonitoring is stopped.");
                    break;
                }
                catch (Exception ex)
                {
                    await Logger.WriteErrorAsync(ex.ToString());
                    throw ex;
                }

                if (MachineInformationService.IsCharging)
                {
                    await Logger.WriteDebugAsync($"WorkTimeMonitoring: Battery is charging: {MachineInformationService.IsCharging}");

                    var message = $"Battery life statistics: {workTime.Hours}h:{workTime.Minutes}m:{workTime.Seconds}s";
                    await SendReportAsync(message);

                    if (workTime.ToString() != emptyTimeSpan.ToString())
                    {
                        ResourcesService.SetResource(workTimeKey, emptyTimeSpan);
                        ResourcesService.SetResource(lastWorkTimeKey, workTime.ToString());
                    }

                    while (MachineInformationService.IsCharging)
                    {
                        // Waiting charging.
                        await Task.Delay(1000);
                    }
                }
                else
                {
                    var time = DateTime.Now - currentDateTime;
                    currentDateTime = DateTime.Now;

                    var newWorkTime = workTime + time;

                    ResourcesService.SetResource(workTimeKey, newWorkTime.ToString());
                }

                await Task.Delay(1000);
            }
        }

        #endregion
    }
}
