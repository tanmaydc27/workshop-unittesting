using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.DAL.EFModels
{
    public class Broker
    {
        public int Id { get; protected set; }
        public double AvailableAmount { get; set; }
        public string Name { get; set; }
    }
}
