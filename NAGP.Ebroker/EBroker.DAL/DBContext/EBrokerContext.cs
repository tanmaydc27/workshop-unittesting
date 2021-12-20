using EBroker.DAL.EFModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EBroker.DAL.DBContext
{
    public class EBrokerContext : DbContext
    {
        public EBrokerContext(DbContextOptions<EBrokerContext> options) : base(options)
        {
        }

        public DbSet<Broker> Brokers { get; set; }

        public DbSet<Equity> Equities { get; set; }

        public DbSet<BrokerEquityMapping> BrokerEquities { get; set; }

    }
}
