using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.DAL.EFModels
{
    public class Equity
    {
        public int Id { get; protected set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
