using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Reports
{
    [Serializable]
    public class ClientReport
    {
        public string Name { get; set; }
        
        public string City { get; set; }
        
        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public List<WorkUnitReport> WorkUnits { get; set; } = new List<WorkUnitReport>();
    }
}
