using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class WorkOrder : BaseModel
    {
        public WorkOrder()
        {
            WorkUnits = new Collection<WorkUnit>();
        }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public int WorkAreaId { get; set; }

        public WorkArea WorkArea { get; set; }

        [ForeignKey("ResponsibleEmployee")]
        public int ResponsibleEmployeeId { get; set; }
        public Employee ResponsibleEmployee { get; set; }

        [ForeignKey("SupervisorEmployee")]
        public int SupervisorEmployeeID { get; set; }
        public Employee SupervisorEmployee { get; set; }

        public Collection<WorkUnit> WorkUnits { get; set; }
    }
}