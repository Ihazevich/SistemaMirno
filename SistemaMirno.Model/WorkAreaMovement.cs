using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class WorkAreaMovement : ModelBase
    {
        public int? FromWorkAreaId { get; set; }
        
        [ForeignKey(nameof(FromWorkAreaId))]
        public virtual WorkArea FromWorkArea { get; set; }

        public int? ToWorkAreaId { get; set; }

        [ForeignKey(nameof(ToWorkAreaId))]
        public virtual WorkArea ToWorkArea { get; set; }

        [Required]
        public int WorkUnitId { get; set; }
        
        [ForeignKey(nameof(WorkUnitId))]
        public virtual WorkUnit WorkUnit { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int? ResponsibleId { get; set; }
        
        public virtual Employee Responsible { get; set; }

        public int? SupervisorId { get; set; }

        public virtual Employee Supervisor { get; set; }
    }
}
