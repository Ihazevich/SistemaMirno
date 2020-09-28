// <copyright file="ClientCommunication.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a single communication between the company and a client.
    /// </summary>
    public partial class ClientCommunication : ModelBase
    {
        /// <summary>
        /// Gets or sets the date of the interaction.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Client"/> entity.
        /// </summary>
        [Required]
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets navigation property to the related <see cref="Model.Client"/> entity.
        /// </summary>
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        /// <summary>
        /// Gets or sets the description of the interaction.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [Required]
        public int ResponsibleId { get; set; }

        /// <summary>
        /// Gets or sets navigation property to the related <see cref="Model.Employee"/> entity.
        /// </summary>
        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }
    }
}
