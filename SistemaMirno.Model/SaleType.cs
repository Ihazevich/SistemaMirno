using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class SaleType : BaseModel
    {
        public string Name { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
