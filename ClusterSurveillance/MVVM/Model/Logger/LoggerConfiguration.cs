using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Windows.Media;
namespace ClusterSurveillance.MVVM.Model.Logger
{
    internal class LoggerConfiguration
    {
        public int EventId { get; set; }

        public Dictionary<LogLevel, Color> LogLevels { get; set; } = new Dictionary<LogLevel, Color>()
        {
            [LogLevel.Information] = Colors.Blue,
            [LogLevel.Warning] = Color.FromRgb(1, 1, 0),
            [LogLevel.Error] = Color.FromRgb(1, 0, 0),
        };
    }
}
