using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class EmployeeRole : BaseModel
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        public virtual Collection<Employee> Employees { get; set; }

        [InverseProperty("WorkAreaResponsibleRole")]
        public virtual Collection<WorkArea> WorkAreasResponsibles { get; set; }
        
        [InverseProperty("WorkAreaSupervisorRole")]
        public virtual Collection<WorkArea> WorkAreasSupervisors { get; set; }
    }
}
