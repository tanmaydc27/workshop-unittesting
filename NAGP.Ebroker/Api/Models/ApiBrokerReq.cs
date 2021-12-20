using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class ApiBrokerReq
    {
        public int BrokerId { get; set; }
        public string Code { get; set; }
        public int NoOfShares { get; set; }
    }
}
