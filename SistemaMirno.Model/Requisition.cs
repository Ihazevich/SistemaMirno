﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class Requisition : ModelBase
    {
        /// <summary>
        /// Gets or sets the date the requisition was created.
        /// </summary>
        [Required]
        public DateTime RequestedDate { get; set; }

        [Required]
        public string Priority { get; set; }

        public DateTime? TargetDate { get; set; }

        [Required]
        public bool Fulfilled { get; set; }

        /// <summary>
        /// Gets or sets the date the requisition was fullfilled.
        /// </summary>
        public DateTime? FulfilledDate { get; set; }

        [Required]
        public bool IsForStock { get; set; }

        public int? ClientId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        public int? SaleId { get; set; }

        [ForeignKey(nameof(SaleId))]
        public virtual Sale Sale { get; set; }

        /// <summary>
        /// Gets or sets the collection of work units that belong to the requisition.
        /// </summary>
        [ForeignKey(nameof(WorkUnit.RequisitionId))]
        public virtual ICollection<WorkUnit> WorkUnits { get; set; } = new HashSet<WorkUnit>();
    }
}
