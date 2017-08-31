namespace EnergyManagementScheduler.WebJob
{
    using System;
    using System.Threading;
    using EnergyManagementScheduler.WebJob.Utilities;
    using Microsoft.Azure.WebJobs;

    public class WebJobHost
    {
        public static void Main()
        {
            while (!ConfigurationSettings.IsValidConfig())
            {
                Console.WriteLine("Invalid application configuration");
                Thread.Sleep(1000);
            }

            var config = new JobHostConfiguration();
            config.UseTimers();
            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
