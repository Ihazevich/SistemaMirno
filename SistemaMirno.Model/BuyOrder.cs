// <copyright file="BuyOrder.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a buy order.
    /// </summary>
    public partial class BuyOrder : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the order.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the number/code of the order.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the id of the <see cref="Model.Provider"/> entity related to the order.
        /// </summary>
        [Required]
        public int ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the <see cref="Model.Provider"/> entity
        /// related to the order.
        /// </summary>
        [ForeignKey(nameof(ProviderId))]
        public virtual Provider Provider { get; set; }

        /// <summary>
        /// Gets or sets the total money ammount of the order.
        /// </summary>
        [Required]
        public long Total { get; set; }

        /// <summary>
        /// Gets or sets the id of the <see cref="Model.Employee"/> entity related to the order.
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the <see cref="Model.Employee"/> entity
        /// related to the order.
        /// </summary>
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the <see cref="Model.Employee"/> entity
        /// related to the order.
        /// </summary>
        [ForeignKey(nameof(BuyOrderUnit.BuyOrderId))]
        public virtual ICollection<BuyOrderUnit> BuyOrderUnits { get; set; } = new HashSet<BuyOrderUnit>();

        /// <summary>
        /// Gets or sets a value indicating whether the ordeer has been paid or not.
        /// </summary>
        [Required]
        public bool IsPaid { get; set; }
    }
}
