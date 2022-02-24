using ClusterSurveillance.MVVM.ViewModel;
using Microsoft.Extensions.Logging;
using System;

namespace ClusterSurveillance.MVVM.Model.Logger
{
    public class LoggerClass
    {
        public LoggingViewModel LoggingViewModel { get; set; }
        public LoggerClass(LoggingViewModel loggingViewModel)
        {
            LoggingViewModel = loggingViewModel;
        }
        private readonly LoggerConfiguration _config = new LoggerConfiguration();

        public bool IsEnabled(LogLevel logLevel) => _config.LogLevels.ContainsKey(logLevel);

        public void Log(LogLevel logLevel, string logMessage)
        {
            string message = $"{DateTime.Now} -- {logMessage}";
            if (!IsEnabled(logLevel))
            {
                return;
            }

            LoggingViewModel.AddLogItem(logLevel.ToString(), message);
        }
    }
}
