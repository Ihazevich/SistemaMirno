// <copyright file="Branch.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class Branch : ModelBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nombre requerido.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Dirección requerida.")]
        [StringLength(100)]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Ciudad requerida.")]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Departamento requerido.")]
        public string Department { get; set; }

        [ForeignKey(nameof(Role.BranchId))]
        public virtual ICollection<Role> Roles { get; set; } = new HashSet<Role>();

        [ForeignKey(nameof(WorkArea.BranchId))]
        public virtual ICollection<WorkArea> WorkAreas { get; set; } = new HashSet<WorkArea>();

        [Required]
        public long Cash { get; set; }
    }
}
