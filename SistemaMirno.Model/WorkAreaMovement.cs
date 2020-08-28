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
        [Required]
        public int WorkAreaId { get; set; }

        [ForeignKey(nameof(WorkAreaId))]
        public virtual WorkArea WorkArea { get; set; }

        [Required]
        public int FromWorkAreaId { get; set; }
        
        [ForeignKey(nameof(FromWorkAreaId))]
        public virtual WorkArea FromWorkArea { get; set; }

        [Required]
        public int ToWorkAreaId { get; set; }

        [ForeignKey(nameof(ToWorkAreaId))]
        public virtual WorkArea ToWorkArea { get; set; }

        [Required]
        public int WorkUnitId { get; set; }
        
        [ForeignKey(nameof(WorkUnitId))]
        public virtual WorkUnit WorkUnit { get; set; }

        [Required]
        public DateTime InDate { get; set; }

        [Required]
        public DateTime OutDate { get; set; }

        [Required]
        public int ResponsibleId { get; set; }
        
        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        [Required]
        public int SupervisorId { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        public virtual Employee Supervisor { get; set; }

        [Required]
        public bool IsNew { get; set; }
        
        [Required]
        public bool IsMove { get; set; }

        [Required]
        public bool IsTransfer { get; set; }
    }
}
