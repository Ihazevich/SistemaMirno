// <copyright file="Vehicle.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class Vehicle : ModelBase
    {
        [Required]
        public string VehicleModel { get; set; }

        public string Year { get; set; }

        public string Patent { get; set; }

        [NotMapped]
        public string FullName => string.Concat(VehicleModel, " - ", Patent);

        public bool IsAvailable { get; set; }

        public DateTime PatentExpiration { get; set; }

        public bool PatentPaid { get; set; }

        public DateTime DinatranExpiration { get; set; }

        public bool DinatranPaid { get; set; }

        public DateTime FireExtinguisherExpiration { get; set; }

        [ForeignKey(nameof(VehicleMaintenanceOrder.VehicleId))]
        public ICollection<VehicleMaintenanceOrder> VehicleMaintenanceOrders { get; set; } = new HashSet<VehicleMaintenanceOrder>();
    }
}
