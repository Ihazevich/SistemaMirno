using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class Requisition : BaseModel
    {
        /// <summary>
        /// Gets or sets the date the requisition was created.
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the date the requisition was fullfilled.
        /// </summary>
        public DateTime FullfilledDate { get; set; }

        /// <summary>
        /// Gets or sets the id of the client the requisition has been assigned to.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the requisition's client.
        /// </summary>
        public virtual Client Client { get; set; }

        /// <summary>
        /// Gets or sets the collection of work units that belong to the requisition.
        /// </summary>
        public virtual ICollection<WorkUnit> WorkUnits { get; set; }
    }
}
