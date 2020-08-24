using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public partial class Vehicle : ModelBase
    {
        [Required]
        public string Model { get; set; }

        [Required]
        public int Year { get; set; }
    }
}
