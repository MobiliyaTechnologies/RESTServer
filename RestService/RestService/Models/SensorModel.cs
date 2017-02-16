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
        public Nullable<double> X { get; set; }
        public Nullable<double> Y { get; set; }
    }
}