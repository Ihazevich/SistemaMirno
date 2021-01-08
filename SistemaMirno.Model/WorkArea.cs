// <copyright file="WorkArea.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a work area in the production line.
    /// </summary>
    public class WorkArea : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the work area.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Requerido")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Branch"/> entity.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public int BranchId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Branch"/> entity.
        /// </summary>
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        /// <summary>
        /// Gets or sets the position of the area in the navigation panel.
        /// </summary>
        [Required(ErrorMessage = "Requerido")]
        public string Position { get; set; }

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

        public bool IsPassthrough { get; set; }

        public int? PassthroughWorkAreaId { get; set; }

        [ForeignKey(nameof(PassthroughWorkAreaId))]
        public virtual WorkArea PassthroughWorkArea { get; set; }

        public bool CanBeDeliveredFrom { get; set; }

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