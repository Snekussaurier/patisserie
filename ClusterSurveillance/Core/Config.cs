using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ClusterSurveillance.Core
{
    public class Config : ObservableObject
    {
        private const string IP_REG_EX = @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$";

        private bool _autoStart;

        public bool AutoStart
        {
            get { return _autoStart; }
            set
            {
                _autoStart = value;
                OnPropertyChanged();
            }
        }

        private bool _sendUsageData;

        public bool SendUsageData
        {
            get { return _sendUsageData; }
            set
            {
                _sendUsageData = value;
                OnPropertyChanged();
            }
        }

        private bool _showWeeklyTips;

        public bool ShowWeeklyTips
        {
            get { return _showWeeklyTips; }
            set
            {
                _showWeeklyTips = value;
                OnPropertyChanged();
            }
        }

        private string _ipAdress;

        public string IPAdress
        {
            get { return _ipAdress ?? "0.0.0.0"; }
            set
            {
                _ipAdress = value.ToString();
                ValidIPAdress = Regex.Match(value.ToString(), IP_REG_EX).Success;
                OnPropertyChanged();
            }
        }

        private bool _validIPAdress;

        public bool ValidIPAdress
        {
            get { return _validIPAdress; }
            set
            {
                _validIPAdress = value;
                OnPropertyChanged();
            }
        }


        private int _port;

        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        private bool _autoReconnectBroker;

        public bool AutoReconnectBroker
        {
            get { return _autoReconnectBroker; }
            set { _autoReconnectBroker = value; }
        }


        private int _reconnectTime;

        public int ReconnectTime
        {
            get { return _reconnectTime; }
            set
            {
                _reconnectTime = value;
                OnPropertyChanged();
            }
        }

        private bool _autoCheckUpdates;

        public bool AutoCheckUpdates
        {
            get { return _autoCheckUpdates; }
            set
            {
                _autoCheckUpdates = value;
                OnPropertyChanged();
            }
        }

        private bool _autoDownloadUpdates;

        public bool AutoDownloadUpdates
        {
            get { return _autoDownloadUpdates; }
            set
            {
                _autoDownloadUpdates = value;
                OnPropertyChanged();
            }
        }
    }
}
