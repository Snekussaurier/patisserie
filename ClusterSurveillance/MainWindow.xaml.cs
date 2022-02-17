using System;
using System.Windows;
using System.Windows.Input;


namespace ClusterSurveillance
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.MaxHeight = SystemParameters.WorkArea.Height + 7;
            this.MaxWidth = SystemParameters.WorkArea.Width + 12;
            InitializeComponent();
        }

        private void TitleBarLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            this.Hide();
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void NotifyIcon(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
                this.Show();
                this.Activate();
                this.Topmost = true;  // important
                this.Topmost = false; // important
                this.Focus();         // important
            }
        }

        private void Shutdown(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
