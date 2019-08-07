using System.Threading.Tasks;
using ControlNet.TelegramBotApi.Models;
using ControlNet.TelegramBotApi.Models.Services.BotService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace ControlNet.TelegramBotApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITelegramBotClient>(t => new TelegramBotClient(token: BotSettings.Token));
            services.AddTransient<IBot, Bot>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var serviceProvider = services.BuildServiceProvider();

            // Resolve the services from the service provider.
            var botService = serviceProvider.GetService<IBot>();
            Task.Run(async () => await botService
                .SetWebhookAsync(BotSettings.WebHookUrl + BotSettings.Token));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
