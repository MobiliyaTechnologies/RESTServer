using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{

    public class MeterDetailsModel
    {
        public int Id { get; set; }
        public string PowerScout { get; set; }
        public string Name { get; set; }
        public string Serial { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public string Description { get; set; }
    }

    public class MeterGeneralDataModel
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

    public class MeterMonthlyConsumptionModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string Powerscout { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public string Ligne { get; set; }

        public double Monthly_KWH_Consumption { get; set; }

        public double Monthly_Electric_Cost { get; set; }

        public DateTime Current_Month { get; set; }

        public DateTime Last_Month { get; set; }
    }

    public class MeterURLKey
    {
        public string Weather { get; set; }

        public string QuarterlyConsumption { get; set; }

        public string LastQuarterlyConsumption { get; set; }

        public string MonthlyConsumption { get; set; }

        public string LastMonthlyConsumption { get; set; }

        public string WeeklyConsumption { get; set; }

        public string LastWeeklyConsumption { get; set; }

        public string DailyConsumption { get; set; }

        public string YesterdayConsumption { get; set; }

        public string MonthlyKWh { get; set; }

        public string MonthlyCost { get; set; }

        public string DayWiseConsumption { get; set; }

        public string PeriodWiseConsumption { get; set; }

        public string Report { get; set; }
    }

    public class MeterMonthWiseConsumption
    {
        public string PowerScout { get; set; }

        public string Name { get; set; }

        public MonthList MonthWiseConsumption { get; set; }

        public MeterMonthWiseConsumption()
        {
            MonthWiseConsumption = new MonthList();
        }
    }

    public class MonthList
    {
        public double Jan { get; set; }

        public double Feb { get; set; }

        public double Mar { get; set; }

        public double Apr { get; set; }

        public double May { get; set; }

        public double Jun { get; set; }

        public double Jul { get; set; }

        public double Aug { get; set; }

        public double Sep { get; set; }

        public double Oct { get; set; }

        public double Nov { get; set; }

        public double Dec { get; set; }
    }

    public class MeterWeekWiseMonthlyConsumption
    {
        public string PowerScout { get; set; }

        public List<Double> WeekWiseConsumption { get; set; }

        public MeterWeekWiseMonthlyConsumption()
        {
            WeekWiseConsumption = new List<double>();
        }
    }
}