using System.Collections.Generic;

namespace EnergyManagementScheduler.WebJob.Contracts
{
    public class Value
    {
        public List<string> ColumnNames { get; set; }

        public List<string> ColumnTypes { get; set; }

        public List<List<string>> Values { get; set; }
    }
}