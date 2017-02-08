using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Models.Statistics.Charts
{
    public class BaseChart<T>
    {
        public IList<string> Labels { get; set; }
        public IList<T> Data { get; set; }
    }
}
