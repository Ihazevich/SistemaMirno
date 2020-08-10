using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a connection between two Work Areas.
    /// </summary>
    public class AreaConnection : BaseModel
    {
        /// <summary>
        /// Gets or sets the ID of the Work Area that acts as the origin of the connection.
        /// </summary>
        [Required]
        public int WorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Work Area that acts as the origin of the connection.
        /// </summary>
        public virtual WorkArea WorkArea { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Work Area that acts as the detination of the connection.
        /// </summary>
        [Required]
        [ForeignKey("ConnectedWorkArea")]
        public int ConnectedWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Work Area that acts as the origin of the connection.
        /// </summary>
        public virtual WorkArea ConnectedWorkArea { get; set; }
    }
}