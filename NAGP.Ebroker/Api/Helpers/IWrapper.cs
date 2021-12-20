using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public interface IWrapper
    {
        public bool IsValidDayTime(DateTime dateTime);

        public IHost DataBaseMigrate()
        {

        }
    }
}
