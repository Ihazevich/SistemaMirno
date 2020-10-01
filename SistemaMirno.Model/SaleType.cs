// <copyright file="SaleType.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace SistemaMirno.Model
{
    public class SaleType : ModelBase
    {
        public string Name { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
