// <copyright file="WorkArea.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// A class representing a work area.
    /// </summary>
    public class WorkArea : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkArea"/> class.
        /// </summary>
        public WorkArea()
        {
            WorkUnits = new Collection<WorkUnit>();
            IncomingWorkOrders = new Collection<WorkOrder>();
            OutgoingWorkOrders = new Collection<WorkOrder>();
            AreaConnections = new Collection<AreaConnection>();
            IncomingAreaConnections = new Collection<AreaConnection>();
        }

        /// <summary>
        /// Gets or sets the name of the work area.
        /// </summary>
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number that indicates the index of the work area in the navigation panel and in reports.
        /// </summary>
        [Required]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Role assigned to be the responsible's role of the work area.
        /// </summary>
        [ForeignKey("WorkAreaResponsibleRole")]
        public int? WorkAreaResponsibleRoleId { get; set; }

        /// <summary>
        /// Gets or sets the Role assigned to be the responsible's role of the work area.
        /// </summary>
        public virtual EmployeeRole WorkAreaResponsibleRole { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Role assigned to be the supervisor's role of the work area.
        /// </summary>
        [ForeignKey("WorkAreaSupervisorRole")]
        public int? WorkAreaSupervisorRoleId { get; set; }

        /// <summary>
        /// Gets or sets the Role assigned to be the supervisor's role of the work area.
        /// </summary>
        public virtual EmployeeRole WorkAreaSupervisorRole { get; set; }

        /// <summary>
        /// Gets or sets the collection of connections that originate from the work area.
        /// </summary>
        public virtual Collection<AreaConnection> AreaConnections { get; set; }

        /// <summary>
        /// Gets or sets the collection of connections that arrive to the work area.
        /// </summary>
        [InverseProperty("ConnectedWorkArea")]
        public virtual Collection<AreaConnection> IncomingAreaConnections { get; set; }

        /// <summary>
        /// Gets or sets the collection of work orders that were destined to the work area.
        /// </summary>
        [InverseProperty("DestinationWorkArea")]
        public virtual Collection<WorkOrder> IncomingWorkOrders { get; set; }

        /// <summary>
        /// Gets or sets the collection of work orders that originated from the work area.
        /// </summary>
        [InverseProperty("OriginWorkArea")]
        public virtual Collection<WorkOrder> OutgoingWorkOrders { get; set; }

        /// <summary>
        /// Gets or sets the collection of work units currently in the work area.
        /// </summary>
        public virtual Collection<WorkUnit> WorkUnits { get; set; }

        /// <summary>
        /// Gets or sets if the work area is included in the InProcess report.
        /// </summary>
        [Required]
        public bool ReportsInProcess { get; set; }
    }
}