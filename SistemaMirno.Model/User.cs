using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a system user.
    /// </summary>
    public class User : BaseModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Access Level of the user.
        /// </summary>
        public int AccessLevel { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        public string Password { get; set; }
    }
}
