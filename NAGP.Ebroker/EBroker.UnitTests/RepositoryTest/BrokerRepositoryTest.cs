using EBroker.DAL;
using EBroker.DAL.DBContext;
using EBroker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using EFModels = EBroker.DAL.EFModels;

namespace EBroker.UnitTests.RepositoryTest
{
    public class BrokerRepositoryTest
    {
        DbContextOptions<EBrokerContext> options;
        public BrokerRepositoryTest()
        {
            options = new DbContextOptionsBuilder<EBrokerContext>().UseInMemoryDatabase(databaseName: "EBrokerDatabse").Options;

            using (var context=new EBrokerContext(options) )
            {
                context.Equities.Add(new EFModels.Equity
                {
                    Code = "TARP",
                    Name = "Tarsons",
                    Price = 1000
                });

                context.Equities.Add(new EFModels.Equity
                {
                    Code = "TATAPO",
                    Name = "Tata Power",
                    Price = 1000
                });

                context.Equities.Add(new EFModels.Equity
                {
                    Code = "TATAMO",
                    Name = "Tata Motors",
                    Price = 1000
                });

                context.Brokers.Add(new EFModels.Broker
                {
                    AvailableAmount = 70000,
                    Name = "Tanmay"
                });

                context.BrokerEquities.Add(new EFModels.BrokerEquityMapping
                {
                    BrokerId = 1,
                    EquityCode = "TARP",
                    AllocatedShares=20
                });

                context.BrokerEquities.Add(new EFModels.BrokerEquityMapping
                {
                    BrokerId = 1,
                    EquityCode = "TARP",
                    AllocatedShares = 20
                });

                context.BrokerEquities.Add(new EFModels.BrokerEquityMapping
                {
                    BrokerId = 1,
                    EquityCode = "TATAMO",
                    AllocatedShares = 20
                });
                context.SaveChanges();

            }
        }

        [Fact]
        public void GetBrokerInfoByID_Return_Broker()
        {
            int brokerID = 1;
            using (var context = new EBrokerContext(options))
            {
                BrokerRepository brokerRepository = new BrokerRepository(context);
                var broker = brokerRepository.GetBrokerInfoByID(brokerID);
                Assert.Equal("Tanmay", broker.Name);

            }
        }

        [Fact]
        public void GetBrokerInfoByID_Returns_NoBroker()
        {
            int brokerID = 100;
            using (var context = new EBrokerContext(options))
            {
                BrokerRepository brokerRepository = new BrokerRepository(context);
                var broker = brokerRepository.GetBrokerInfoByID(brokerID);
                Assert.Null(broker);

            }
        }

        [Fact]
        public void IsValidEquityForBroker_Returns_True()
        {
            using (var context = new EBrokerContext(options))
            {
                BrokerRepository brokerRepository = new BrokerRepository(context);
                var response = brokerRepository.IsValidEquityForBroker(1,"TARP");
                Assert.True(response);

            }
        }

        [Fact]
        public void IsValidEquityForBroker_Returns_False()
        {
            using (var context = new EBrokerContext(options))
            {
                BrokerRepository brokerRepository = new BrokerRepository(context);
                var response = brokerRepository.IsValidEquityForBroker(1, "DUMMY");
                Assert.False(response);

            }
        }

        [Fact]
        public void BuyEquity_BrokerExistingEquity()
        {
            int brokerID = 1;
            Equity equity = new Equity { Code = "TARP", NoOfShares = 10 ,Price=100};
            using (var context = new EBrokerContext(options))
            {
                var existingQuantity = context.BrokerEquities.Where(x => x.BrokerId == brokerID && x.EquityCode == equity.Code).FirstOrDefault().AllocatedShares;
                BrokerRepository brokerRepository = new BrokerRepository(context);
                brokerRepository.BuyEquity(brokerID, equity);

                var updatedBrokerEquity= context.BrokerEquities.Where(x => x.BrokerId == brokerID && x.EquityCode == equity.Code).FirstOrDefault();
                
                Assert.Equal(existingQuantity + equity.NoOfShares , updatedBrokerEquity.AllocatedShares);

            }
        }

        [Fact]
        public void BuyEquity_BrokerNewEquity()
        {
            int brokerID = 1;
            Equity equity = new Equity { Code = "TATAPO", NoOfShares = 10 ,Price=100};
            using (var context = new EBrokerContext(options))
            {
                BrokerRepository brokerRepository = new BrokerRepository(context);
                brokerRepository.BuyEquity(brokerID, equity);

                var newBrokerEquity = context.BrokerEquities.Where(x => x.BrokerId == brokerID && x.EquityCode == equity.Code).FirstOrDefault();

                Assert.Equal(equity.NoOfShares, newBrokerEquity.AllocatedShares);

            }
        }

        [Fact]
        public void SellEquity()
        {
            int brokerID = 1;
            Equity equity = new Equity { Code = "TATAMO", NoOfShares = 10 };
            using (var context = new EBrokerContext(options))
            {
                var existingQuantity = context.BrokerEquities.Where(x => x.BrokerId == brokerID && x.EquityCode == equity.Code).FirstOrDefault().AllocatedShares;
                BrokerRepository brokerRepository = new BrokerRepository(context);
                brokerRepository.SellEquity(brokerID, equity);

                var updatedBrokerEquity = context.BrokerEquities.Where(x => x.BrokerId == brokerID && x.EquityCode == equity.Code).FirstOrDefault();

                Assert.Equal(existingQuantity - equity.NoOfShares, updatedBrokerEquity.AllocatedShares);
            }
        }

        [Fact]
        public void AddFunds()
        {
            int brokerID = 1;
            double amount = 200000;
            using (var context = new EBrokerContext(options))
            {
                var existingAmount = context.Brokers.Where(x => x.Id == brokerID).FirstOrDefault().AvailableAmount;
                BrokerRepository brokerRepository = new BrokerRepository(context);
                brokerRepository.AddFunds(brokerID, amount);

                var newAvailableAmount = context.Brokers.Where(x => x.Id == brokerID).FirstOrDefault().AvailableAmount;

                Assert.Equal(existingAmount + amount, newAvailableAmount);
            }
        }

    }
}
