using System;
using System.Threading;
using System.Threading.Tasks;
using ControlNet.MonitoringService.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ControlNet.MonitoringService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IMonitoring _monitoring;

        public Worker(ILogger<Worker> logger, IMonitoring monitoring)
        {
            _logger = logger;
            _monitoring = monitoring;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _monitoring.StartServiceAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTime.Now}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
