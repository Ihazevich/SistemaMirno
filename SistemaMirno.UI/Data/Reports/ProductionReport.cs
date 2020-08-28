using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Reports
{
    [Serializable]
    public class ProductionReport
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string WorkArea { get; set; }
        public long Total { get; set; }
        public List<WorkUnitReport> WorkUnits { get; set; }
    }
}
