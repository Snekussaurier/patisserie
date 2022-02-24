using ClusterSurveillance.Core;
using System;
using System.Timers;

namespace ClusterSurveillance.MVVM.Model
{
    public class Client : ObservableObject
    {
        private Timer _timer;

        public string ClientId { get; set; }
        private string _name;

        public string Name
        {
            get { return _name; }
            set {if (!string.IsNullOrEmpty(value)) _name = value;
                else _name = "Defaukt NAme";
            }
        }
        public string Adress { get; set; } = "#UNKNOWN!";
        public DateTime Created { get; set; }

        private bool _status;

        public bool Status
        {
            get { return _status; }
            set { _status = value;
                OnPropertyChanged();
            }
        }


        public Client(string clientId, string name, string adress, DateTime created)
        {
            ClientId = clientId;
            Name = name;
            if(!string.IsNullOrEmpty(adress)) Adress = adress;
            Created = created;
            Status = true;
            _timer = new();
            _timer.Interval = 10000;
            _timer.Elapsed += OnTimerElapsed;
            _timer.Enabled = true;
        }

        private void OnTimerElapsed(Object source, ElapsedEventArgs e)
        {
            Status = false;
        }

        public void GetMessage()
        {
            Status = true;
            _timer.Stop();
            _timer.Start();
        }
    }
}
