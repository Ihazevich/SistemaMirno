// <copyright file="SaleType.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a sale type/category.
    /// </summary>
    public class SaleType : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Sale"/> entities.
        /// </summary>
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
