using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class HardwareCategory : ModelBase
    {
        [Required]
        public string Name { get; set; }

        [ForeignKey(nameof(Hardware.HardwareCategoryId))]
        public virtual ICollection<Hardware> HardwareCollection { get; set; } = new HashSet<Hardware>();
    }
}
