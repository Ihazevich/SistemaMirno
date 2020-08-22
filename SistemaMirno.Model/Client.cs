using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class Client : ModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RUC { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Department { get; set; }
        public virtual ICollection<Requisition> Requisitions { get; set; }
    }
}
