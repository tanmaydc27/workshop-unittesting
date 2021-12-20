using EBroker.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;


namespace EBroker.UnitTests.RepositoryTest
{
    public class EBrokerContextSeedTest
    {
        DbContextOptions<EBrokerContext> options;
        private readonly Mock<ILogger<EBrokerContextSeed>> _logger;
        public EBrokerContextSeedTest()
        {
            options = new DbContextOptionsBuilder<EBrokerContext>().UseInMemoryDatabase(databaseName: "EBrokerData").Options;
            _logger = new Mock<ILogger<EBrokerContextSeed>>();


        }

        [Fact]
        public void EBrokerContextSeed_Test()
        {
            bool isExist;
            using (var context = new EBrokerContext(options))
            {
                EBrokerContextSeed.SeedAsync(new EBrokerContext(options), _logger.Object).GetAwaiter().GetResult();
                isExist = context.Brokers.Any();
            }
            Assert.True(isExist);
        }
    }
}
