using System.Collections.Generic;

namespace Model.Models.Statistics.Charts
{
    public class BaseChart<T>
    {
        public IList<string> Labels { get; set; }
        public IList<T> Data { get; set; }
    }
}
