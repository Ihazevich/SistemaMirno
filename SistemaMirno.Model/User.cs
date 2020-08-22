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
    public class User : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Employee Id.
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the Employee.
        /// </summary>
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
    }
}
