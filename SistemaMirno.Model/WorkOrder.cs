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

        [ForeignKey("OriginWorkArea")]
        public int? OriginWorkAreaId { get; set; }

        public virtual WorkArea OriginWorkArea { get; set; }

        [ForeignKey("DestinationWorkArea")]
        public int DestinationWorkAreaId { get; set; }

        public virtual WorkArea DestinationWorkArea { get; set; }

        [ForeignKey("ResponsibleEmployee")]
        public int? ResponsibleEmployeeId { get; set; }
        public virtual Employee ResponsibleEmployee { get; set; }

        [ForeignKey("SupervisorEmployee")]
        public int? SupervisorEmployeeId { get; set; }
        public virtual Employee SupervisorEmployee { get; set; }

        public virtual Collection<WorkOrderUnit> WorkOrderUnits { get; set; }
    }
}