using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Dto.Dashboard
{
    public enum ChartType
    {
        pie,
        doughnut,
        polarArea,
        bar,
        line
    }

    public enum Months
    {
        Jan = 1,
        Feb,
        Mar,
        Apr,
        May,
        June,
        Jul,
        August,
        Sep,
        Oct,
        Nove,
        December,
    }

    public class ChartJsDataset
    {
        public string Label { get; set; } = "Default Label";
        public string[] Data { get; set; }
        public string[] BackgroundColor { get; set; }
        public string Stack { get; set; }
        public string[] Labels { get; set; }

    }


}
