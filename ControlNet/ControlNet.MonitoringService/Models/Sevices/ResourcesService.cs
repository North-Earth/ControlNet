using ControlNet.Logger;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace ControlNet.MonitoringService.Models.Sevices
{
    public class ResourcesService : IResourcesService
    {
        #region Fields
        private Configuration Configuration { get; }

        private ILogger Logger { get; }

        private const string settingsSection = "appSettings";

        #endregion

        #region Constructors

        public ResourcesService(ILogger logger)
        {
            string applicationName =
                Environment.GetCommandLineArgs()[0];

            string path = System.IO.Path.Combine(
                Environment.CurrentDirectory, applicationName);

            Configuration = ConfigurationManager.OpenExeConfiguration(exePath: path);
            Logger = logger;
        }

        #endregion

        #region Methods

        public string GetResource(string resourceKey)
        {
            if (Configuration.AppSettings.Settings.AllKeys.Any(k => k == resourceKey))
                return Configuration.AppSettings.Settings[resourceKey].Value;

            Logger.WriteWarningAsync($"Resource {resourceKey} is not found.");
            return null;
        }

        public void SetResource(string resourceKey, string resourceValue)
        {
            // Check if not exists key.
            if (!Configuration.AppSettings.Settings.AllKeys.Any(k => k == resourceKey))
            {
                Configuration.AppSettings.Settings.Add(new KeyValueConfigurationElement(resourceKey, resourceValue));
                Logger.WriteInformationAsync($"Resource {resourceKey} has been added.");
            }
            else // Make changes.
            {
                Configuration.AppSettings.Settings[resourceKey].Value = resourceValue;
                //Logger.WriteInformationAsync($"Resource {resourceKey} has been edited.");
            }

            // Save to apply changes.
            Configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(settingsSection);
        }

        public void AddResource(string resourceKey, string resourceValue)
        {
            throw new NotImplementedException(); //TODO;
        }

        #endregion


    }
}
