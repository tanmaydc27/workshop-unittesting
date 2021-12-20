using EBroker.DAL.DBContext;
using EFModels=EBroker.DAL.EFModels;
using EBroker.Domain;
using EBroker.Interfaces.repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EBroker.DAL
{
    public class BrokerRepository : IBrokerRepository
    {
        private readonly EBrokerContext _dbContext;
        public BrokerRepository(EBrokerContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void BuyEquity(int brokerID, Equity equity)
        {
            var broker = _dbContext.Brokers.Where(x => x.Id == brokerID).FirstOrDefault();
            var brokerEquity = _dbContext.BrokerEquities.Where(x => x.BrokerId == brokerID && x.EquityCode == equity.Code).FirstOrDefault();
            if (brokerEquity == null)
            {
                EFModels.BrokerEquityMapping brokerEquityMapping = new EFModels.BrokerEquityMapping { BrokerId = brokerID, EquityCode = equity.Code, AllocatedShares = equity.NoOfShares };
                _dbContext.BrokerEquities.Add(brokerEquityMapping);
            }
            else
            {
                brokerEquity.AllocatedShares = brokerEquity.AllocatedShares + equity.NoOfShares;
            }
            broker.AvailableAmount = broker.AvailableAmount - (equity.Price * equity.NoOfShares);
            _dbContext.SaveChanges();
        }

        public void AddFunds(int brokerID, double amount)
        {
            var broker = _dbContext.Brokers.Where(x => x.Id == brokerID).FirstOrDefault();
            broker.AvailableAmount = broker.AvailableAmount + amount;
            _dbContext.SaveChanges();
        }

        public double SellEquity(int brokerID, Equity equity)
        {
            var brokerEquity = _dbContext.BrokerEquities.Where(x => x.BrokerId == brokerID && x.EquityCode == equity.Code).FirstOrDefault();
            var soldEquity = _dbContext.Equities.Where(x => x.Code == equity.Code).FirstOrDefault();
            brokerEquity.AllocatedShares = brokerEquity.AllocatedShares - equity.NoOfShares;
            _dbContext.SaveChanges();
            return (soldEquity.Price * equity.NoOfShares);
        }

        public Broker GetBrokerInfoByID(int brokerID)
        {
            return _dbContext.Brokers.Where(x => x.Id == brokerID).Select(br => new Broker { AvailableFund = br.AvailableAmount, Id = br.Id, Name = br.Name }).FirstOrDefault();
        }
        public bool IsValidEquityForBroker( int brokerID, string equityCode)
        {
            var brokerEquity = _dbContext.BrokerEquities.Where(x => x.BrokerId == brokerID && x.EquityCode == equityCode).FirstOrDefault();
            return brokerEquity == null ? false : true;
        }
    }
}
