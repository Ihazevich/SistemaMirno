using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing the AreaConnection.
    /// </summary>
    public class AreaConnection
    {
        public int Id { get; set; }

        [Required]
        public int FromAreaId { get; set; }

        [Required]
        public int ToAreaId { get; set; }
    }
}
