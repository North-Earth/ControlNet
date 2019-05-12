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

        private static readonly string _queryBatteryInfo = "SELECT * FROM Win32_Battery";

        #endregion

        #region Methods

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

        public static string GetBatteryStatus()
        {
            var objectName = "BatteryStatus";

            var batteryCharge = ManagementInformationService
                .GetManagementInformation(_queryBatteryInfo, objectName);

            return batteryCharge;
        }

        #endregion

    }
}
