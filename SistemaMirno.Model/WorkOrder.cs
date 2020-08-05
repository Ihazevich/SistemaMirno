using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class WorkOrder : BaseModel
    {
        public WorkOrder()
        {
            WorkOrderUnits = new Collection<WorkOrderUnit>();
        }

        public DateTime StartTime { get; set; }

        public DateTime? FinishTime { get; set; }

        [ForeignKey("LeavingWorkArea")]
        public int LeavingWorkAreaId { get; set; }

        public virtual WorkArea LeavingWorkArea { get; set; }

        [ForeignKey("EnteringWorkArea")]
        public int EnteringWorkAreaId { get; set; }

        public virtual WorkArea EnteringWorkArea { get; set; }

        [ForeignKey("ResponsibleEmployee")]
        public int? ResponsibleEmployeeId { get; set; }
        public virtual Employee ResponsibleEmployee { get; set; }

        [ForeignKey("SupervisorEmployee")]
        public int? SupervisorEmployeeId { get; set; }
        public virtual Employee SupervisorEmployee { get; set; }

        public virtual Collection<WorkOrderUnit> WorkOrderUnits { get; set; }
    }
}