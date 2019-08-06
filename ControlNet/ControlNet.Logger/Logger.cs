using System;
using System.IO;
using System.Threading.Tasks;

namespace ControlNet.Logger
{
    public class Logger : ILogger
    {
        #region Fields

        private string FileLogPath { get; } = null;

        private enum Status
        {
            Information,
            Warning,
            Error,
            Debug
        }

        #endregion

        #region Constructors

        public Logger() { }

        #endregion

        #region Methods
        private async Task Write(string message)
        {
            throw new NotImplementedException();
        }

        private async Task Write(string message, string prefix, ConsoleColor prefixColor)
        {
            var dateTime = DateTime.Now.ToString();
            var log = $"[{dateTime}] - {message}";

            WriteToConsole(log, prefix, prefixColor);
            await WriteToFileAsync($"{prefix}{log}");
        }

        private void WriteToConsole(string message, string prefix, ConsoleColor prefixColor)
        {
            Console.ForegroundColor = prefixColor;
            Console.Write($"{prefix}: ");
            Console.ResetColor();

            var logMessage = message.Replace("\n", "\n\t");
            Console.WriteLine(logMessage);
        }

        private async Task WriteToFileAsync(string message)
        {
            var currentDate = DateTime.Now.ToShortDateString();

            var appDir = AppDomain.CurrentDomain.BaseDirectory;
            var logsDir = $@"{appDir}Logs";
            if (!Directory.Exists(logsDir))
                Directory.CreateDirectory(logsDir);
            var path = $@"{logsDir}\log({currentDate}).txt";
            var logMessage = message.Replace("\n", "\n\t");

            await File.AppendAllTextAsync(path, $"{logMessage}\n");
        }

        public async Task WriteInformationAsync(string message)
            => await Write(message, Status.Information.ToString(), ConsoleColor.White);

        public async Task WriteWarningAsync(string message)
            => await Write(message, Status.Warning.ToString(), ConsoleColor.Yellow);

        public async Task WriteErrorAsync(string message)
            => await Write(message, Status.Error.ToString(), ConsoleColor.Red);

        public async Task WriteDebugAsync(string message)
            => await Write(message, Status.Debug.ToString(), ConsoleColor.Gray);

        #endregion
    }
}
