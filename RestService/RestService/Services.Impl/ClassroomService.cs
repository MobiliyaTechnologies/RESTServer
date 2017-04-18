namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Mappings;
    using RestService.Models;

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
            var classroomDetails = this.dbContext.ClassroomDetails;
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