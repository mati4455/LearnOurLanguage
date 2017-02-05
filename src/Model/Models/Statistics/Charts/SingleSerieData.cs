using System.Collections.Generic;

namespace Model.Models.Statistics.Charts
{
    public class SingleSerieData
    {
        public string Label { get; set; }
        public IList<decimal> Data { get; set; }
    }
}
