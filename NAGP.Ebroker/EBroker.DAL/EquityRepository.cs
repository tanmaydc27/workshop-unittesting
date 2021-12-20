using EBroker.DAL.DBContext;
using EBroker.Domain;
using EBroker.Interfaces.repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EBroker.DAL
{
    public class EquityRepository : IEquityRepository
    {
        private readonly EBrokerContext _dbContext;
        public EquityRepository(EBrokerContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public Equity GetEquityByCode(string code)
        {
            return _dbContext.Equities.Where(x => x.Code == code).Select(y => new Equity { Code = y.Code, Price = y.Price }).FirstOrDefault();
        }
    }
}
