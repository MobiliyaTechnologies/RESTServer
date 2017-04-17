namespace RestService.Services.Impl
{
    using System;
    using RestService.Models;
    using RestService.Utilities;

    public sealed class PowerBIService : IPowerBIService
    {
        PowerBIGeneralURLModel IPowerBIService.GetPowerBIGeneralURL()
        {
            return new PowerBIGeneralURLModel();
        }

        MeterURLKeyModel IPowerBIService.GetPowerBIUrl(string meterSerial)
        {
            if (PowerBIUtil.MeterURLKeyDictionary.ContainsKey(meterSerial))
            {
                return PowerBIUtil.MeterURLKeyDictionary[meterSerial];
            }
            else
            {
                return null;
            }
        }
    }
}