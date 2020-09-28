// <copyright file="Branch.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a company branch.
    /// </summary>
    public class Branch : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the branch.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nombre requerido.")]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the branch.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Dirección requerida.")]
        [StringLength(100)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city where the branch is located.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ciudad requerida.")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the department where the branch is located.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Departamento requerido.")]
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Role"/> entities.
        /// </summary>
        [ForeignKey(nameof(Role.BranchId))]
        public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkArea"/> entities.
        /// </summary>
        [ForeignKey(nameof(WorkArea.BranchId))]
        public virtual ICollection<WorkArea> WorkAreas { get; set; } = new HashSet<WorkArea>();

        /// <summary>
        /// Gets or sets the ammount of money currently in the branch.
        /// </summary>
        [Required]
        public long Cash { get; set; }
    }
}
