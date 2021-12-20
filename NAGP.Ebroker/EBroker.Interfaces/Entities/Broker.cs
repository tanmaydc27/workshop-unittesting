using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.Domain
{
    public class Broker
    {
        public Broker()
        {
            Equities = new List<Equity>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double AvailableFund { get; set; }
        public List<Equity>Equities {get;set;}
    }
}
