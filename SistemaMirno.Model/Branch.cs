using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class Branch : BaseModel
    {
        public string Name { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }

        public virtual ICollection<WorkArea> WorkAreas { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
