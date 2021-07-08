using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WanoSivuv3.Models
{
    public class Charts
    {
        public int Name { get; set; }
        public int prodName { get; set; }

    }
    public class pCharts
    {
        public string type { get; set; }
        public string username { get; set; }
    }

    public class BarChart
    {
        public IList<Charts> barPoints { get; set; }
    }
    public class PieChart
    {
        public IList<pCharts> piePoints { get; set; }
    }

    public class StatisticsViewModel
    {
        public BarChart barChart { get; set; }
        public PieChart pieChart { get; set; }
    }
}
