﻿// <copyright file="Delivery.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class Delivery : ModelBase
    {
        [Required]
        public int SaleId { get; set; }

        [ForeignKey(nameof(SaleId))]
        public virtual Sale Sale { get; set; }

        [Required]
        public int DeliveryOrderId { get; set; }

        [ForeignKey(nameof(DeliveryOrderId))]
        public virtual DeliveryOrder DeliveryOrder { get; set; }

        public bool Delivered { get; set; }

        public bool Cancelled { get; set; }

        public DateTime? DeliveredOn { get; set; }

        public string Details { get; set; }

        public string ReasonNotDelivered { get; set; }

        public virtual ICollection<DeliveryUnit> DeliveryUnits { get; set; } = new HashSet<DeliveryUnit>();
    }
}
