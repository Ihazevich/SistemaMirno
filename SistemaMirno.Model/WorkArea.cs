using System.Collections.Generic;
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
            WorkOrders = new Collection<WorkOrder>();
            FromAreaConnections = new Collection<AreaConnection>();
            AreaConnectionsTo = new Collection<AreaConnection>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        [ForeignKey("WorkAreaResponsibleRole")]
        public int? WorkAreaResponsibleRoleId { get; set; }
        public EmployeeRole WorkAreaResponsibleRole { get; set; }

        [ForeignKey("WorkAreaSupervisorRole")]
        public int? WorkAreaSupervisorRoleId { get; set; }
        public EmployeeRole WorkAreaSupervisorRole { get; set; }

        [InverseProperty("FromWorkArea")]
        public Collection<AreaConnection> FromAreaConnections { get; set; }

        [InverseProperty("ToWorkArea")]
        public Collection<AreaConnection> AreaConnectionsTo { get; set; }

        public Collection<WorkUnit> WorkUnits { get; set; }
        public Collection<WorkOrder> WorkOrders { get; set; }

    }
}