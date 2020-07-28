using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class WorkUnit : BaseModel
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int MaterialId { get; set; }

        [Required]
        public int ColorId { get; set; }

        [Required]
        public int ProductionAreaId { get; set; }

        [Required]
        public int ResponsibleId { get; set; }
        
        [Required]
        public int SupervisorId { get; set; }
    }
}
