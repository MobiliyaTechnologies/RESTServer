namespace EnergyManagementScheduler.WebJob.Logger
{
    using System;
    using System.Collections.Generic;
    using EnergyManagementScheduler.WebJob.Utilities;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;

    public static class ApplicationInsightsLogger
    {
        private static readonly TelemetryClient TelemetryClient;

        static ApplicationInsightsLogger()
        {
            TelemetryClient = new TelemetryClient();
            TelemetryClient.InstrumentationKey = ConfigurationSettings.ApplicationInsightsInstrumentationKey;
        }

        public static void LogException(Exception ex, Dictionary<string, string> logProperties = null)
        {
            try
            {
                var exceptionTelemetry = new ExceptionTelemetry(ex);

                if (logProperties != null)
                {
                    foreach (var logProperty in logProperties)
                    {
                        exceptionTelemetry.Properties.Add(logProperty.Key, logProperty.Value);
                    }
                }

                TelemetryClient.TrackException(exceptionTelemetry);
                TelemetryClient.Flush();
            }
            catch (Exception)
            {
                // ignore to avoid original exception overriding.
            }
        }
    }
}
