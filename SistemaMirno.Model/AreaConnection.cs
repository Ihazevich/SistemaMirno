using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing the AreaConnection.
    /// </summary>
    public class AreaConnection
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("FromWorkArea")]
        public int FromWorkAreaId { get; set; }
        public WorkArea FromWorkArea { get; set; }

        [Required]
        [ForeignKey("ToWorkArea")]
        public int ToWorkAreaId { get; set; }
        public WorkArea ToWorkArea { get; set; }
    }
}