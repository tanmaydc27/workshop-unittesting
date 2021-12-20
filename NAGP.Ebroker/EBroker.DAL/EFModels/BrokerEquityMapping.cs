using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.DAL.EFModels
{
    public class BrokerEquityMapping
    {
        public int Id { get; protected set; }
        public int  BrokerId { get; set; }
        public string EquityCode { get; set; }
        public int AllocatedShares { get; set; }
    }
}
