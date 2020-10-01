// <copyright file="Provider.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a company supply/hardware provider.
    /// </summary>
    public partial class Provider : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the provider's phone number.
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the provider's address.
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Purchase"/> entities.
        /// </summary>
        [ForeignKey(nameof(BuyOrder.ProviderId))]
        public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.DatedCheck"/> entities.
        /// </summary>
        [ForeignKey(nameof(DatedCheck.ProviderId))]
        public virtual ICollection<DatedCheck> DatedChecks { get; set; } = new HashSet<DatedCheck>();
    }
}
