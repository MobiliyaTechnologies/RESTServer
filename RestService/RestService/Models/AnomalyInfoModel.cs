using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class AnomalyInfoModel
    {

        public string PowerScout { get; set; }
        public Nullable<double> Temperature { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
        public Nullable<double> Visibility { get; set; }
        public string days { get; set; }
        public Nullable<double> kW_System { get; set; }
        public Nullable<double> ScoredLabels { get; set; }
        public Nullable<double> ScoredProbabilities { get; set; }
        public int Id { get; set; }
        public string Breaker_details { get; set; }
    }
}