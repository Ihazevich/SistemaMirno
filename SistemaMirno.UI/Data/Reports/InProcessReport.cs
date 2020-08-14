using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Reports
{
    /// <summary>
    /// A class representing the Units in Process report.
    /// </summary>
    [Serializable]
    public class InProcessReport
    {
        /// <summary>
        /// Gets or sets the date and time the report was generated.
        /// </summary>
        public string Datetime { get; set; }

        /// <summary>
        /// Gets or sets the work areas included in the report.
        /// </summary>
        public List<WorkAreaReport> WorkAreas { get; set; }
    }
}
