using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class MeterDataModel
    {
        public string Name { get; set; }

        public string Serial { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public double DailyConsumption { get; set; }

        public double DailyElectricCost { get; set; }

        public double MonthlyConsumption { get; set; }

        public double MonthlyElectricCost { get; set; }

    }
}