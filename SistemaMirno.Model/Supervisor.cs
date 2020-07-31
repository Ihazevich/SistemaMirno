using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class Supervisor
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        public ICollection<WorkOrder> WorkOrders { get; set; }
    }
}
