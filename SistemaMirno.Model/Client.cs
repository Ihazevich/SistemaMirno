// <copyright file="Client.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a company client.
    /// </summary>
    public partial class Client : ModelBase
    {
        /// <summary>
        /// Gets or sets the full name of the client.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the RUC code of the client.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Ruc { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the client.
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the address of the client.
        /// </summary>
        [StringLength(100)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city where the client is located.
        /// </summary>
        [Required]
        [StringLength(30)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the department where the client is located.
        /// </summary>
        [StringLength(20)]
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the email of the client.
        /// </summary>
        [StringLength(30)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client is retail or not.
        /// </summary>
        [Required]
        public bool IsRetail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the client is a wholesaler or not.
        /// </summary>
        [Required]
        public bool IsWholesaler { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Sale"/> entities.
        /// </summary>
        [ForeignKey(nameof(Sale.ClientId))]
        public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.ClientCommunication"/> entities.
        /// </summary>
        [ForeignKey(nameof(ClientCommunication.ClientId))]
        public virtual ICollection<ClientCommunication> ClientCommunications { get; set; } = new HashSet<ClientCommunication>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Requisition"/> entities.
        /// </summary>
        [ForeignKey(nameof(Requisition.ClientId))]
        public virtual ICollection<Requisition> Requisitions { get; set; } = new HashSet<Requisition>();
    }
}
