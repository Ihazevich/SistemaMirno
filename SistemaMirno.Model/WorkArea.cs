// <copyright file="WorkArea.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a work area.
    /// </summary>
    public class WorkArea : ModelBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public string Position { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public int ResponsibleRoleId { get; set; }

        [ForeignKey(nameof(ResponsibleRoleId))]
        public Role ResponsibleRole { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public int SupervisorRoleId { get; set; }

        [ForeignKey(nameof(SupervisorRoleId))]
        public Role SupervisorRole { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public bool IsFirst { get; set; }

        [Required(ErrorMessage = "Requerido")]
        public bool IsLast { get; set; }
        
        [Required(ErrorMessage = "Requerido")]
        public bool ReportsInProcess { get; set; }

        [ForeignKey(nameof(WorkUnit.CurrentWorkAreaId))]
        public virtual ICollection<WorkUnit> WorkUnits { get; set; } = new HashSet<WorkUnit>();

        [ForeignKey(nameof(WorkAreaConnection.OriginWorkAreaId))]
        public virtual ICollection<WorkAreaConnection> OutgoingConnections { get; set; } = new HashSet<WorkAreaConnection>();

        [ForeignKey(nameof(WorkAreaConnection.DestinationWorkAreaId))]
        public virtual ICollection<WorkAreaConnection> IncomingConnections { get; set; } = new HashSet<WorkAreaConnection>();

        [ForeignKey(nameof(WorkOrder.OriginWorkAreaId))]
        public virtual ICollection<WorkOrder> OutgoingWorkOrders { get; set; } = new HashSet<WorkOrder>();

        [ForeignKey(nameof(WorkOrder.DestinationWorkAreaId))]
        public virtual ICollection<WorkOrder> IncomingWorkOrders { get; set; } = new HashSet<WorkOrder>();

    }
}