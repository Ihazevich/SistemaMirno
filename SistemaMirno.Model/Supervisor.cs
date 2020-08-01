using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class Supervisor
    {
        public Supervisor()
        {
            WorkOrders = new Collection<WorkOrder>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        public Collection<WorkOrder> WorkOrders { get; set; }
    }
}