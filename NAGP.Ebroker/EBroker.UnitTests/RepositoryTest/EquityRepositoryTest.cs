using EBroker.DAL;
using EBroker.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using EFModels = EBroker.DAL.EFModels;

namespace EBroker.UnitTests.RepositoryTest
{
    public class EquityRepositoryTest
    {
        DbContextOptions<EBrokerContext> options;
        public EquityRepositoryTest()
        {
            options = new DbContextOptionsBuilder<EBrokerContext>().UseInMemoryDatabase(databaseName: "EBrokerDatabse").Options;
            using (var context = new EBrokerContext(options))
            {
                context.Equities.Add(new EFModels.Equity
                {
                    Code = "NYKA",
                    Name = "Nyka Cosmetics",
                    Price = 1000
                });
                context.SaveChanges();

            }
        }
        [Fact]
        public void GetBrokerInfoByID_Returns_NoBroker()
        {
            string code = "NYKA";
            using (var context = new EBrokerContext(options))
            {
                EquityRepository equityRepository = new EquityRepository(context);
                var equity = equityRepository.GetEquityByCode(code);
                Assert.Equal(code, equity.Code);

            }
        }
    }
}
