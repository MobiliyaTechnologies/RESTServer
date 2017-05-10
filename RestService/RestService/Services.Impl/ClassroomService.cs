namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Mappings;
    using RestService.Models;
    using RestService.Utilities;

    public sealed class ClassroomService : IClassroomService, IDisposable
    {
        private readonly PowerGridEntities dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassroomService"/> class.
        /// </summary>
        public ClassroomService()
        {
            this.dbContext = new PowerGridEntities();
        }

        List<ClassroomModel> IClassroomService.GetAllClassrooms()
        {
            var accessibleBuildings = this.dbContext.Building.WhereActiveAccessibleBuilding().Select(b => b.BuildingName.Trim());
            var classroomDetails = this.dbContext.ClassroomDetails.Where(c => accessibleBuildings.Any(b => b.Equals(c.Building.Trim(), StringComparison.InvariantCultureIgnoreCase)));

            return new ClassroomModelMapping().Map(classroomDetails).ToList();
        }

        List<ClassroomModel> IClassroomService.GetClassroomByBuilding(int buildingId)
        {
            var accessibleBuilding = this.dbContext.Building.WhereActiveAccessibleBuilding(data => data.BuildingID == buildingId).FirstOrDefault();

            if (accessibleBuilding == null)
            {
                return new List<ClassroomModel>();
            }

            var classroomDetails = this.dbContext.ClassroomDetails.Where(c => c.Building.Trim().Equals(accessibleBuilding.BuildingName.Trim(), StringComparison.InvariantCultureIgnoreCase));

            return new ClassroomModelMapping().Map(classroomDetails).ToList();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }
    }
}