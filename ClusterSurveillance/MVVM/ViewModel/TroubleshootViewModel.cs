using ClusterSurveillance.Core;

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
                System.Diagnostics.Process.Start("https://gitlab.com/Snekussaurier/patisserie/-/wikis/home");
            });
        }
    }
}
