using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public  class Helper
    {
        public static bool IsValidTimeDay(DateTime dateTime)
        {
            TimeSpan startTime = new TimeSpan(9, 0, 0);
            TimeSpan endTime = new TimeSpan(17, 0, 0);
            if (dateTime.TimeOfDay < startTime || dateTime.TimeOfDay > endTime)
                return false;
            if (dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Sunday)
                return false;
            else
                return true;
        }
    }
}
