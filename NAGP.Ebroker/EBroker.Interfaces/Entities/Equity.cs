using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.Domain
{
    public class Equity
    {
        public string Code { get; set; }
        //public string Name { get; set; }
        public double Price { get; set; }
        public int NoOfShares { get; set; }
    }
}
