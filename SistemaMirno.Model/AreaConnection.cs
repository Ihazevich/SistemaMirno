using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class AreaConnection
    {
        public int Id { get; set; }

        [Required]
        public int FromAreaId { get; set; }
        
        [Required]
        public int ToAreaId { get; set; }
    }
}
