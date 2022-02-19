using ClusterSurveillance.Core;
using ClusterSurveillance.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace ClusterSurveillance.MVVM.ViewModel
{
    public class LoggingViewModel : ObservableObject
    {
        private ObservableCollection<LogItem> _logList;

        public ObservableCollection<LogItem> MyProperty
        {
            get { return _logList; }
            set { _logList = value;
                App.Current.Dispatcher.BeginInvoke((Action)delegate
                {
                    OnPropertyChanged();
                });
            }
        }

        public ObservableCollection<LogItem> LogList { get; set; }

        public LoggingViewModel()
        {
            LogList = new ObservableCollection<LogItem>();
        }
        public void AddLogItem(string logLevel, string log)
        {
            App.Current.Dispatcher.BeginInvoke((Action)delegate
            {
                LogList.Add(new LogItem { LogLevel = logLevel, Log = log});
            });
        }
    }

    public class LogItem
    {
        public string LogLevel { get; set; }
        public string Log { get; set; }
    }
}
