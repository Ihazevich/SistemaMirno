using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing the AreaConnection.
    /// </summary>
    public class AreaConnection : BaseModel
    {
        [Required]
        public int WorkAreaId { get; set; }

        public virtual WorkArea WorkArea { get; set; }

        [Required]
        [ForeignKey("ConnectedWorkArea")]
        public int ConnectedWorkAreaId { get; set; }

        public virtual WorkArea ConnectedWorkArea { get; set; }
    }
}