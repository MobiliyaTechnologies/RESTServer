using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Utilities
{
    public class Constants
    {
        public enum StatusCode
        {
            Ok = 200,
            Error = 0
        };

        public enum DayofWeek
        {
            Mon = 1,
            Tue = 2,
            Wed = 3,
            Thu = 4,
            Fri = 5,
            Sat = 6,
            Sun = 7
        }
    }
}