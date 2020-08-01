using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class ProductionArea : BaseModel
    {
        public ProductionArea()
        {
            Responsibles = new Collection<Responsible>();
            WorkUnits = new Collection<WorkUnit>();
            WorkOrders = new Collection<WorkOrder>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        public Collection<Responsible> Responsibles { get; set; }
        public Collection<WorkUnit> WorkUnits { get; set; }
        public Collection<WorkOrder> WorkOrders { get; set; }
    }
}