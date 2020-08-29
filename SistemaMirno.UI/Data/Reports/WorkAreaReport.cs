using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Reports
{
    /// <summary>
    /// A class representing the data of a work area used for reports.
    /// </summary>
    [Serializable]
    public class WorkAreaReport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaReport"/> class.
        /// </summary>
        public WorkAreaReport()
        {
            WorkUnits = new List<WorkUnitReport>();
        }

        /// <summary>
        /// Gets or sets the name of the work area.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets if the report includes the production price of work units.
        /// </summary>
        public bool IncludePrice { get; set; }

        /// <summary>
        /// Gets or sets the total production value of the work area.
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets the work units in the work area.
        /// </summary>
        public ICollection<WorkUnitReport> WorkUnits { get; set; }
    }
}
