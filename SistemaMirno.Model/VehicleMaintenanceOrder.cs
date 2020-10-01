// <copyright file="VehicleMaintenanceOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class VehicleMaintenanceOrder : ModelBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(7000)]
        public string Details { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        [Required]
        [StringLength(200)]
        public string Place { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public Vehicle Vehicle { get; set; }
    }
}