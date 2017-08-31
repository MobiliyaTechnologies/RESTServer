namespace EnergyManagementScheduler.WebJob.Contracts
{
    using System.Collections.Generic;

    public class Input1
    {
        public List<string> ColumnNames { get; set; }

        public List<List<string>> Values { get; set; }
    }
}
