namespace RestService.Models
{
    using System;

    public class AnomalyInfoModel
    {

        public string PowerScout { get; set; }

        public double? Temperature { get; set; }

        public DateTime? Timestamp { get; set; }

        public double? Visibility { get; set; }

        public string Days { get; set; }

        public double? KW_System { get; set; }

        public double? ScoredLabels { get; set; }

        public double? ScoredProbabilities { get; set; }

        public int Id { get; set; }

        public string Breaker_details { get; set; }
    }
}