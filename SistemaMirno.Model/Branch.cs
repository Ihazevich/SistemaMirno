using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class Branch : ModelBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        public string Department { get; set; }

        [ForeignKey(nameof(Role.BranchId))]
        public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();

        [ForeignKey(nameof(WorkArea.BranchId))]
        public virtual ICollection<WorkArea> WorkAreas { get; set; } = new HashSet<WorkArea>();
    }
}
