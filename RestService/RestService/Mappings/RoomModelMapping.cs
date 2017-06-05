namespace RestService.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class RoomModelMapping
    {
        public IQueryable<RoomModel> Map(IQueryable<RoomDetail> source)
        {
            return from s in source
                   select new RoomModel
                   {
                       RoomDescription = s.Room_Desc,
                       RoomId = s.Room_Id,
                       RoomName = s.Room_Name,
                       Building = s.Building,
                       Breaker_details = s.Breaker_details,
                       X = s.X ?? default(double),
                       Y = s.Y ?? default(double)
                   };
        }

        public RoomModel Map(RoomDetail source)
        {
            return source == null ? null : this.Map(new List<RoomDetail> { source }.AsQueryable()).First();
        }
    }
}