// <copyright file="WorkAreaConnection.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a connection between two <see cref="Model.WorkArea"/> entities.
    /// </summary>
    public class WorkAreaConnection : ModelBase
    {
        /// <summary>
        /// Gets or sets the id of the Work Area that acts as the origin of the connection.
        /// </summary>
        [Required]
        public int OriginWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Work Area that acts as the origin of the connection.
        /// </summary>
        [ForeignKey(nameof(OriginWorkAreaId))]
        public virtual WorkArea OriginWorkArea { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Work Area that acts as the detination of the connection.
        /// </summary>
        [Required]
        public int DestinationWorkAreaId { get; set; }

        /// <summary>
        /// Gets or sets the Work Area that acts as the origin of the connection.
        /// </summary>
        [ForeignKey(nameof(DestinationWorkAreaId))]
        public virtual WorkArea DestinationWorkArea { get; set; }
    }
}