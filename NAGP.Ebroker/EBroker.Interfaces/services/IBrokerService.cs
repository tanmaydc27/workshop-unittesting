using EBroker.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.Interfaces.services
{
    public interface IBrokerService
    {
        public bool BuyEquity(int brokerID, Equity equity);

        public double AddFunds(int brokerID, double amount);

        public (bool isValidEquity, double refundAmount) SellEquity(int brokerID, Equity equity);

        public bool IsBrokerExist(int brokerID);

    }
}
