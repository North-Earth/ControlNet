using System;
using System.Configuration;
using System.Linq;

namespace ControlNet.MonitoringService.Models.Sevices
{
    public static class ResourcesService
    {
        public static Configuration Configuration { get; } = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        #region Methods

        public static string GetResource(string resourceKey)
        {
            if (Configuration.AppSettings.Settings.AllKeys.Any(k => k == resourceKey))
                return Configuration.AppSettings.Settings[resourceKey].Value;
            
            return null;
        }

        public static void SetResource(string resourceKey, string resourceValue)
        {
            // Check if not exists key.
            if (!Configuration.AppSettings.Settings.AllKeys.Any(k => k == resourceKey))
            {
                Configuration.AppSettings.Settings.Add(new KeyValueConfigurationElement(resourceKey, resourceValue));
            }
            else // Make changes.
            {
                Configuration.AppSettings.Settings[resourceKey].Value = resourceValue;
            }

            // Save to apply changes.
            Configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        #endregion


    }
}
