using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class Responsible
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        public int ProductionAreaId { get; set; }

        public ProductionArea ProductionArea { get; set; }

        public ICollection<WorkOrder> WorkOrders { get; set; }
    }
}