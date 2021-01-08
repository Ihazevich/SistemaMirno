// <copyright file="User.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a system user.
    /// </summary>
    public partial class User : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nombre de usuario requerido")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contraseña requerida")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password verification string.
        /// </summary>
        [NotMapped]
        public string PasswordVerification { get; set; }

        /// <summary>
        /// Gets or sets id of the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets navigation property to the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [Required]
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has access to the accounting module.
        /// </summary>
        [Required]
        public bool HasAccessToAccounting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has access to the production module.
        /// </summary>
        [Required]
        public bool HasAccessToProduction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has access to the logistics module.
        /// </summary>
        [Required]
        public bool HasAccessToLogistics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has access to the sales module.
        /// </summary>
        [Required]
        public bool HasAccessToSales { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has access to the human resources module.
        /// </summary>
        [Required]
        public bool HasAccessToHumanResources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is a system administrator.
        /// </summary>
        [Required]
        public bool IsSystemAdmin { get; set; }
    }
}
