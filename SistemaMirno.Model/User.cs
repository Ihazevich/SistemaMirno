// <copyright file="User.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a system user.
    /// </summary>
    public partial class User : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nombre de usuario requerido")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contraseña requerida")]
        public string Password { get; set; }

        [NotMapped]
        public string PasswordVerification { get; set; }

        /// <summary>
        /// Gets or sets the Employee Id.
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the Employee.
        /// </summary>
        [Required]
        public virtual Employee Employee { get; set; }

        [Required]
        public bool HasAccessToAccounting { get; set; }

        [Required]
        public bool HasAccessToProduction { get; set; }

        [Required]
        public bool HasAccessToLogistics { get; set; }

        [Required]
        public bool HasAccessToSales { get; set; }

        [Required]
        public bool HasAccessToHumanResources { get; set; }

        [Required]
        public bool IsSystemAdmin { get; set; }
    }
}
