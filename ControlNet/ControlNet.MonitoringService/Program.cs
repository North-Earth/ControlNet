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

namespace ControlNet.MonitoringService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggerFactory => loggerFactory.AddEventLog()) // Send logs to the Windows Event Log.
                .UseServiceBaseLifetime()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
