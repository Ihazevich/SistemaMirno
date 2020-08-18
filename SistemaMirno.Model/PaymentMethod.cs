using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class PaymentMethod : BaseModel
    {
        public string Name { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
