// <copyright file="Client.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class Client : ModelBase
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        public string Ruc { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(30)]
        public string City { get; set; }

        [StringLength(20)]
        public string Department { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        public bool IsRetail { get; set; }

        [Required]
        public bool IsWholesaler { get; set; }

        [ForeignKey(nameof(Sale.ClientId))]
        public virtual ICollection<Sale> Sales { get; set; } = new HashSet<Sale>();

        [ForeignKey(nameof(ClientCommunication.ClientId))]
        public virtual ICollection<ClientCommunication> ClientCommunications { get; set; } = new HashSet<ClientCommunication>();

        [ForeignKey(nameof(Requisition.ClientId))]
        public virtual ICollection<Requisition> Requisitions { get; set; } = new HashSet<Requisition>();
    }
}
