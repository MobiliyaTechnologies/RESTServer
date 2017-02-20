using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class SensorModel
    {
        public int Sensor_Id { get; set; }
        public string Sensor_Name { get; set; }
        public Nullable<int> Class_Id { get; set; }
        public string Class_Name { get; set; }
        public Nullable<double> X { get; set; }
        public Nullable<double> Y { get; set; }
        public Nullable<double> Class_X { get; set; }
        public Nullable<double> Class_Y { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Brightness { get; set; }
    }
}