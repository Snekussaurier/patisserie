using ClusterSurveillance.Core;
using ClusterSurveillance.MVVM.Model.Logger;
using System.Diagnostics;
using System.Windows;

namespace ClusterSurveillance.MVVM.ViewModel
{
    internal class TroubleshootViewModel : ObservableObject
    {
        private readonly LoggerClass _logger;
        public RelayCommand OpenSupport { get; set; }

        public RelayCommand RestartApplication { get; set; }

        public TroubleshootViewModel(LoggerClass logger)
        {
            _logger  = logger;

            OpenSupport = new RelayCommand(o =>
            {
                try
                {
                    Process.Start("https://gitlab.com/Snekussaurier/patisserie/-/wikis/home");
                }
                catch (System.Exception e)
                {
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, e.ToString());
                }
            });
            RestartApplication = new RelayCommand(o =>
            {
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            });
        }
    }
}
