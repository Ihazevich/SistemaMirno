using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Reports
{
    [Serializable]
    public class WorkOrderReport
    {
        public WorkOrderReport()
        {
            WorkUnits = new List<WorkUnitReport>();
        }

        public int Id { get; set; }

        public string CreationDateTime { get; set; }

        public string OriginWorkArea { get; set; }

        public string DestinationWorkArea { get; set; }

        public string Responsible { get; set; }

        public string Supervisor { get; set; }

        public List<WorkUnitReport> WorkUnits { get; set; }
    }
}
