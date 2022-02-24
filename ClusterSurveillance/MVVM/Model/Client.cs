using ClusterSurveillance.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterSurveillance.MVVM.Model
{
    public class Client : ObservableObject
    {
        public string ClientId { get; set; }
        public string Name { get; set; } = "Default Name";
        public string Adress { get; set; } = "#UNKNOWN!";
        public DateTime Created { get; set; }

        public Client(string clientId, string name, string adress, DateTime created)
        {
            ClientId = clientId;
            if(!string.IsNullOrEmpty(name)) Name = name;
            if(!string.IsNullOrEmpty(adress)) Adress = adress;
            Created = created;
        }
    }
}
