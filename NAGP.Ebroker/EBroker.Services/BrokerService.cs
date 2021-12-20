using EBroker.Domain;
using EBroker.Interfaces.repository;
using EBroker.Interfaces.services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.Services
{
    public class BrokerService : IBrokerService
    {
        private readonly IBrokerRepository _brokerRepository;

        private readonly IEquityRepository _equityRepository;
        public BrokerService(IBrokerRepository brokerRepository,IEquityRepository equityRepository)
        {
            _brokerRepository = brokerRepository;
            _equityRepository = equityRepository;
        }
        public bool BuyEquity(int brokerID, Equity equity)
        {
            var brokerInfo = _brokerRepository.GetBrokerInfoByID(brokerID);

            var equityInfo = _equityRepository.GetEquityByCode(equity.Code);
            if(brokerInfo.AvailableFund < equity.NoOfShares * equityInfo.Price)
            {
                return false;
            }
            else
            {
                equity.Price = equityInfo.Price;
                _brokerRepository.BuyEquity(brokerID,equity);
            }

            return true;
        }

        public double AddFunds(int brokerID, double amount)
        {
            if(amount>100000)
            {
                amount = amount - (.0005 * amount);
            }
            _brokerRepository.AddFunds(brokerID, amount);

            return amount;
        }

        public (bool isValidEquity , double refundAmount)  SellEquity(int brokerID, Equity equity)
        {
            bool isValidEquity = _brokerRepository.IsValidEquityForBroker(brokerID, equity.Code);
            if(isValidEquity)
            {
                var amountAdded = _brokerRepository.SellEquity(brokerID, equity);
                double brokerageAmount = amountAdded * .0005 > 20 ? amountAdded * .0005 : 20;
                double finalAmountToBeAdded = amountAdded - brokerageAmount;
                _brokerRepository.AddFunds(brokerID, finalAmountToBeAdded);
                return (true,finalAmountToBeAdded);
            }
            return (false,0);

        }

        public bool IsBrokerExist(int brokerID)
        {
            var broker = _brokerRepository.GetBrokerInfoByID(brokerID);
            return broker == null ? false : true;
        }
    }
}
