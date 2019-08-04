using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace ControlNet.MonitoringService.Models.Sevices
{
    public static class MachineInformationService
    {
        #region Fields

        private const string _queryBatteryInfo = "SELECT * FROM Win32_Battery";

        #endregion

        #region Methods

        public static string GetAppDirectory()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            return dir;
        }

        public static string GetMachineName()
        {
            var machineName = Environment.MachineName;
            return machineName;
        }

        public static string GetBatteryCharge()
        {
            var objectName = "EstimatedChargeRemaining";

            var batteryCharge = ManagementInformationService
                .GetManagementInformation(_queryBatteryInfo, objectName);

            return batteryCharge;
        }

        public static bool IsCharging
            => GetBatteryStatus() != "1";

        public static string GetBatteryStatus()
            => BatteryStatus();

        private static string BatteryStatus()
        {
            var objectName = "BatteryStatus";

            var batteryStatus = ManagementInformationService
                .GetManagementInformation(_queryBatteryInfo, objectName);

            return batteryStatus;
        }

        #endregion

    }
}
