using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class Supply : ModelBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Brand { get; set; }

        [Required]
        [StringLength(100)]
        public string MeasureUnit { get; set; }

        [Required]
        public int SupplyCategoryId { get; set; }
    }
}
