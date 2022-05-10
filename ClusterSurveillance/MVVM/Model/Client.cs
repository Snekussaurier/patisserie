using ClusterSurveillance.Core;
using System;
using System.Globalization;
using System.Timers;

namespace ClusterSurveillance.MVVM.Model
{
    public class Client : ObservableObject
    {
        private Timer _timer;

        public int ClientId { get; set; }
        private string _name;

        public string Name
        {
            get { return _name; }
            set {if (!string.IsNullOrEmpty(value)) _name = value;
                else _name = "#NONAME";
            }
        }
        public string Adress { get; set; } = "#UNKNOWN!";
        public DateTime Created { get; set; }

        private int _status;

        public int Status
        {
            get { return _status; }
            set { _status = value;
                OnPropertyChanged();
            }
        }

        // Current values
        private float _temperature;

        public float Temperature
        {
            get { return _temperature; }
            set { _temperature = value;
                OnPropertyChanged();
            }
        }

        private float _humidity;

        public float Humidity
        {
            get { return _humidity; }
            set { _humidity = value;
                OnPropertyChanged();
            }
        }

        // Limits
        private float _tempLimit;

        public float TempLimit
        {
            get { return _tempLimit; }
            set { _tempLimit = value; }
        }

        private float _humidLimit;

        public float HumidLimit
        {
            get { return _humidLimit; }
            set { _humidLimit = value; }
        }



        public Client(int clientId, string name, string adress, DateTime created)
        {
            ClientId = clientId;
            Name = name;
            if(!string.IsNullOrEmpty(adress)) Adress = adress;
            Created = created;
            Status = 0;
            _timer = new();
            _timer.Interval = 10000;
            _timer.Elapsed += OnTimerElapsed;
            _timer.Enabled = true;
        }

        private void OnTimerElapsed(Object source, ElapsedEventArgs e)
        {
            Status = 0;
        }

        public void GetMessage(string payload)
        {
            Status = 1;
            _timer.Stop();
            _timer.Start();

            string[] values = payload.Split(':');

            SetTemperature(float.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat));
            SetHumidity(float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat));
        }

        public void SetAlarm()
        {
            Status = 2;
        }

        public void SetTemperature(float temperature)
        {
            Temperature = temperature;
        }
       
        public void SetHumidity(float humidity)
        {
            Humidity = humidity;
        }
    }
}
