using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ControlNet.MonitoringService.Models;
using ControlNet.MonitoringService.Models.Sevices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ControlNet.MonitoringService
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;

        private readonly ILogger<Worker> _winLogger; //Windows events logger.

        private readonly IMonitoring _monitoring;

        private readonly IResourcesService _resourcesService;

        private readonly ControlNet.Logger.ILogger _loger;

        public Worker(IConfiguration configuration,
            ILogger<Worker> winLogger,
            IMonitoring monitoring,
            IResourcesService resourcesService,
            ControlNet.Logger.ILogger logger)
        {
            _configuration = configuration;
            _winLogger = winLogger;
            _monitoring = monitoring;
            _resourcesService = resourcesService;
            _loger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _loger.WriteInformationAsync("The service is running!");

            await StartAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _winLogger.LogInformation($"Worker running at: {DateTime.Now}");
                await Task.Delay(10000, stoppingToken);
            }
        }

        private async Task StartAsync()
        {
            bool isValid = false;

            while (!isValid)
            {
                try
                {
                    await _monitoring.StartServiceAsync();
                    await _loger.WriteInformationAsync("The monitoring system is running!");
                    isValid = true;
                }
                catch (Exception ex)
                {
                    await _loger.WriteWarningAsync("Unsuccessful the monitoring system startup.");
                    await _loger.WriteErrorAsync(ex.Message + "\n" + ex.ToString());

                    isValid = false;

                    // Waiting before restarting in case of unsuccessful start.
                    await _loger.WriteWarningAsync("Waiting for the monitoring system to restart.");
                    await Task.Delay(millisecondsDelay: 30000);
                    await _loger.WriteWarningAsync("Restart the monitoring system.");
                }
            }
        }
    }
}
