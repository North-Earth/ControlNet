using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace ControlNet.MonitoringService.Models.Sevices
{
    public static class ManagementInformationService
    {
        #region Methods

        public static string GetManagementInformation(string query, string objectName)
        {
            var managementInfo = GetManagementInformation(query);

            var propertyValue = managementInfo?.Where(prop => prop.Name == objectName)?.FirstOrDefault()?.Value.ToString();

            return propertyValue;
        }

        public static IEnumerable<PropertyData> GetManagementInformation(string query)
        {
            var objectQuery = new ObjectQuery(query);
            var searcher = new ManagementObjectSearcher(objectQuery);
            var collection = searcher.Get();

            var properties = collection.OfType<ManagementObject>().Select(obj => obj.Properties.OfType<PropertyData>())?.FirstOrDefault();

            return properties;
        }

        #endregion
    }
}
