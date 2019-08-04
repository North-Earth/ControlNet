using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ControlNet.Logger
{
    public interface ILogger
    {
        #region Methods

        Task WriteInformationAsync(string message);

        Task WriteWarningAsync(string message);

        Task WriteErrorAsync(string message);

        Task WriteDebugAsync(string message);

        #endregion
    }
}
