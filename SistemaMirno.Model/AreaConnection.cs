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
        [ForeignKey("FromWorkArea")]
        public int? FromWorkAreaId { get; set; }
        public virtual WorkArea FromWorkArea { get; set; }

        [Required]
        [ForeignKey("ToWorkArea")]
        public int? ToWorkAreaId { get; set; }
        public virtual WorkArea ToWorkArea { get; set; }
    }
}