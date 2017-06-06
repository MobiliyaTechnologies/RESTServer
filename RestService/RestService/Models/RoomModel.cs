namespace RestService.Models
{
    public class RoomModel
    {
        public int RoomId { get; set; }

        public string RoomName { get; set; }

        public int SensorId { get; set; }

        public string Building { get; set; }

        public double X { get; set; }

        public double Y { get; set; }
    }
}