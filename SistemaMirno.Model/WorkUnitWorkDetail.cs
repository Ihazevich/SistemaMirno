// <copyright file="WorkUnitWorkDetail.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class WorkUnitWorkDetail : ModelBase
    {
        [Required]
        public int WorkUnitId { get; set; }

        [ForeignKey(nameof(WorkUnitId))]
        public virtual WorkUnit WorkUnit { get; set; }

        [Required]
        public int WorkOrderId { get; set; }

        [ForeignKey(nameof(WorkOrderId))]
        public virtual WorkOrder WorkOrder { get; set; }

        [Required]
        public int WorkAreaId { get; set; }

        [ForeignKey(nameof(WorkAreaId))]
        public virtual WorkArea WorkArea { get; set; }

        [Required]
        public DateTime StartedDateTime { get; set; }

        public DateTime? FinishedDateTime { get; set; }

        [Required]
        public int ResponsibleId { get; set; }

        [ForeignKey(nameof(ResponsibleId))]
        public virtual Employee Responsible { get; set; }

        [Required]
        public int SupervisorId { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        public virtual Employee Supervisor { get; set; }
    }
}
