// <copyright file="WorkUnit.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a product with a certain material and color.
    /// </summary>
    public class WorkUnit : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnit"/> class.
        /// </summary>
        public WorkUnit()
        {
            WorkOrderUnits = new Collection<WorkOrderUnit>();
        }

        /// <summary>
        /// Gets or sets the Id of the product of the work unit.
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// gets or sets the product of the work unit.
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Gets or sets the Id of the material of the work unit.
        /// </summary>
        [Required]
        public int MaterialId { get; set; }

        /// <summary>
        /// Gets or sets the material of the work unit.
        /// </summary>
        public virtual Material Material { get; set; }

        /// <summary>
        /// Gets or sets the Id of the color of the work unit.
        /// </summary>
        [Required]
        public int ColorId { get; set; }

        /// <summary>
        /// Gets or sets the color of the work unit.
        /// </summary>
        public virtual Color Color { get; set; }

        /// <summary>
        /// Gets or sets the id of the work area the work unit is presently in.
        /// </summary>
        [Required]
        public int WorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the work area the work unit is presently in.
        /// </summary>
        public virtual WorkArea WorkArea { get; set; }

        /// <summary>
        /// Gets or sets the collection of Work Order units that point to this work unit.
        /// </summary>
        public virtual Collection<WorkOrderUnit> WorkOrderUnits { get; set; }
    }
}