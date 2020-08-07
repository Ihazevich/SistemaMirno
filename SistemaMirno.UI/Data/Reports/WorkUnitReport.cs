using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Reports
{
    [Serializable]
    public class WorkUnitReport
    {
        public int Quantity { get; set; }

        public string Product { get; set; }

        public string Material { get; set; }

        public string Color { get; set; }
    }
}
