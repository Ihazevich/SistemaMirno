using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class ProductionArea : BaseModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        public ICollection<Responsible> Responsibles { get; set; }
        public ICollection<WorkUnit> WorkUnits { get; set; }
        public ICollection<WorkOrder> WorkOrders { get; set; }
    }
}
