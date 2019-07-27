using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting.WindowsServices;
using ControlNet.MonitoringService.Models.Sevices;
using ControlNet.MonitoringService.Models;

namespace ControlNet.MonitoringService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
            .ConfigureLogging(loggerFactory => loggerFactory.AddEventLog()) // Send logs to the Windows Event Log.
            .UseWindowsService() // Rename UseServiceBaseLifetime to UseWindowsService after update dotnetcore version. https://github.com/aspnet/Extensions/issues/1288
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                // Call other providers here and call AddCommandLine last.
                config.AddCommandLine(args);
            })
            .ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
                services.AddTransient<IMonitoring, Monitoring>();
            });
    }
}
