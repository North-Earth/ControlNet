using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlNet.MonitoringService.Models.Sevices
{
    public static class MachineInformationService
    {

        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public static string GetMachineName()
        {
            var machineName = Environment.MachineName;
            return machineName;
        }

        #endregion

    }
}
