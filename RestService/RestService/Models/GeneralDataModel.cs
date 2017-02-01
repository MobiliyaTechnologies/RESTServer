using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class GeneralDataModel
    {

    }

    public class PowerBIGeneralURL
    {
        private const string monthlyConsumptionKWh = "https://app.powerbi.com/embed?dashboardId=09821193-5c76-4a55-b960-74815806ecac&tileId=37167b28-d3ec-4d9e-b937-405f0b96bb87";

        private const string monthlyConsumptionCost = "https://app.powerbi.com/embed?dashboardId=09821193-5c76-4a55-b960-74815806ecac&tileId=baecd6f4-7ff1-4aea-a882-fdb1dea1f885";

        public string MonthlyConsumptionKWh { get { return monthlyConsumptionKWh; } private set { } }

        public string MonthlyConsumptionCost { get { return monthlyConsumptionCost; } private set { } }
    }
}