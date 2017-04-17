namespace RestService.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class ClassroomModelMapping
    {
        public IQueryable<ClassroomModel> Map(IQueryable<ClassroomDetails> source)
        {
            return from s in source
                   select new ClassroomModel
                   {
                       ClassDescription = s.Class_Desc,
                       ClassId = s.Class_Id,
                       ClassName = s.Class_Name,
                       Building = s.Building,
                       Breaker_details = s.Breaker_details,
                       X = s.X ?? default(double),
                       Y = s.Y ?? default(double)
                   };
        }

        public ClassroomModel Map(ClassroomDetails source)
        {
            return source == null ? null : this.Map(new List<ClassroomDetails> { source }.AsQueryable()).First();
        }
    }
}