using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class Assistance : ModelBase
    {
        [Required]
        public int EmployeeId { get; set; }
        
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string CheckIn { get; set; }

        public string CheckOut { get; set; }
    }
}
