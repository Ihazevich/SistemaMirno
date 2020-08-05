using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class WorkArea : BaseModel
    {
        public WorkArea()
        {
            WorkUnits = new Collection<WorkUnit>();
            IncomingWorkOrders = new Collection<WorkOrder>();
            OutgoingWorkOrders = new Collection<WorkOrder>();
            FromAreaConnections = new Collection<AreaConnection>();
            AreaConnectionsTo = new Collection<AreaConnection>();
        }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        [ForeignKey("WorkAreaResponsibleRole")]
        public int? WorkAreaResponsibleRoleId { get; set; }

        public virtual EmployeeRole WorkAreaResponsibleRole { get; set; }

        [ForeignKey("WorkAreaSupervisorRole")]
        public int? WorkAreaSupervisorRoleId { get; set; }

        public virtual EmployeeRole WorkAreaSupervisorRole { get; set; }

        [InverseProperty("FromWorkArea")]
        public virtual Collection<AreaConnection> FromAreaConnections { get; set; }

        [InverseProperty("ToWorkArea")]
        public virtual Collection<AreaConnection> AreaConnectionsTo { get; set; }

        public virtual Collection<WorkUnit> WorkUnits { get; set; }

        [InverseProperty("EnteringWorkArea")]
        public virtual Collection<WorkOrder> IncomingWorkOrders { get; set; }

        [InverseProperty("LeavingWorkArea")]
        public virtual Collection<WorkOrder> OutgoingWorkOrders { get; set; }

    }
}