using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int ProductionAreaId { get; set; }
        public int ResponsibleId { get; set; }
        public int SupervisorID { get; set; }
    }
}
