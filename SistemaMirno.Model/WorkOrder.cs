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

        public virtual WorkArea WorkArea { get; set; }

        [ForeignKey("ResponsibleEmployee")]
        public int? ResponsibleEmployeeId { get; set; }
        public virtual Employee ResponsibleEmployee { get; set; }

        [ForeignKey("SupervisorEmployee")]
        public int? SupervisorEmployeeID { get; set; }
        public virtual Employee SupervisorEmployee { get; set; }

        public virtual Collection<WorkUnit> WorkUnits { get; set; }
    }
}