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

        private readonly ILogger<Worker> _logger;

        private readonly IMonitoring _monitoring;

        public Worker(IConfiguration configuration, ILogger<Worker> logger, IMonitoring monitoring)
        {
            _configuration = configuration;
            _logger = logger;
            _monitoring = monitoring;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Logger("The service is running!");
            await StartAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTime.Now}");
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
                    await Logger("The monitoring system is running!");
                    isValid = true;
                }
                catch (Exception ex)
                {
                    await Logger("Unsuccessful the monitoring system startup.");
                    await Logger(ex.Message);
                    await Logger(ex.ToString());

                    isValid = false;

                    // Waiting before restarting in case of unsuccessful start.
                    await Logger("Waiting for the monitoring system to restart.");
                    await Task.Delay(millisecondsDelay: 30000);
                    await Logger("Restart the monitoring system.");
                }
            }
        }

        private async Task Logger(string message)
        {
            string text = $"[{DateTime.Now}] {message} \n";

            await File.AppendAllTextAsync(@"C:\Logs\ControlNetLog.txt", text);
        }
    }
}
