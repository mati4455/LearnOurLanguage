using System;
using Model.Models.Database;

namespace Model.Models.Statistics
{
    public class TimeStatistics
    {
        public DateTime Date { get; set; }
        public Game Game { get; set; }
        public decimal MinDuration { get; set; }
        public decimal MaxDuration { get; set; }
        public decimal AvgDuration { get; set; }
    }
}
