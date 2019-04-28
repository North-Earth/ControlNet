using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlNet.MonitoringService.Models
{
    public interface IMonitoring
    {
        #region Methods

        Task StartServiceAsync();

        Task SendReportAsync(string message);

        #endregion
    }
}
