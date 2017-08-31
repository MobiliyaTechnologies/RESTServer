namespace EnergyManagementScheduler.WebJob.Contracts
{
    using System;

    public class PiServerInfo
    {
        public class PiServer
        {
            public int PiServerID { get; set; }

            public string PiServerName { get; set; }

            public string PiServerDesc { get; set; }

            public int CampusID { get; set; }

            public string PiServerURL { get; set; }

            public bool IsActive { get; set; }

            public Nullable<int> CreatedBy { get; set; }

            public Nullable<System.DateTime> CreatedOn { get; set; }

            public Nullable<int> ModifiedBy { get; set; }

            public Nullable<System.DateTime> ModifiedOn { get; set; }

            public bool IsDeleted { get; set; }
        }
    }
}
