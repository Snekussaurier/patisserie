using ClusterSurveillance.Core;
using System.Diagnostics;
using System.Windows;

namespace ClusterSurveillance.MVVM.ViewModel
{
    internal class TroubleshootViewModel : ObservableObject
    {
        public RelayCommand OpenSupport { get; set; }

        public RelayCommand RestartApplication { get; set; }

        public TroubleshootViewModel()
        {
            OpenSupport = new RelayCommand(o =>
            {
                Process.Start("https://gitlab.com/Snekussaurier/patisserie/-/wikis/home");
            });
            RestartApplication = new RelayCommand(o =>
            {
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            });
        }
    }
}
