using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public class Wrapper : IWrapper
    {
        public bool IsValidDayTime(DateTime dateTime)
        {
            return Helper.IsValidTimeDay(dateTime);
        }
    }
}
