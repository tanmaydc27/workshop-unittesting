using EBroker.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.Interfaces.repository
{
    public interface IBrokerRepository
    {
        public void BuyEquity(int brokerID, Equity equity);

        public void AddFunds(int brokerID, double amount);

        public double SellEquity(int brokerID, Equity equity);

        public bool IsValidEquityForBroker(int brokerID, string equityCode);

        public Broker GetBrokerInfoByID(int brokerID);
    }
}
