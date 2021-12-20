using EBroker.DAL.EFModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBroker.DAL.DBContext
{
    public class EBrokerContextSeed
    {
        public static async Task SeedAsync(EBrokerContext ebrokerContext, ILogger<EBrokerContextSeed> logger)
        {            
            if (!ebrokerContext.Equities.Any())
            {
                ebrokerContext.Equities.AddRange(GetPreconfiguredEquities());
                ebrokerContext.SaveChanges();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(EBrokerContext).Name);
            }
            if (!ebrokerContext.Brokers.Any())
            {
                ebrokerContext.Brokers.AddRange(GetPreconfiguredBroker());
                ebrokerContext.SaveChanges();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(EBrokerContext).Name);
            }
        }

        private static IEnumerable<Equity> GetPreconfiguredEquities()
        {
            return new List<Equity>
            {
                new Equity() { Code="TARP",Name="Tarson Power",Price=1000 },
                new Equity() { Code="TATPOW",Name="Tata Power",Price=1000 },
                new Equity() { Code="TARP",Name="Tata Motors",Price=1000 }
            };
        }

        private static IEnumerable<Broker> GetPreconfiguredBroker()
        {
            return new List<Broker>
            {
                new Broker() { AvailableAmount=80000, Name="Tanmay" },
                new Broker() { AvailableAmount=80000, Name="Puneet"  },
                new Broker() { AvailableAmount=80000, Name="Vishal"  }
            };
        }
    }
}
