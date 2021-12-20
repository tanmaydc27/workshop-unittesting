using EBroker.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.Interfaces.repository
{
    public interface IEquityRepository
    {
        public Equity GetEquityByCode(string code);
    }
}
