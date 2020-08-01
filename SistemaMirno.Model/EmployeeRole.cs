using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class EmployeeRole
    {
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        public Collection<Employee> Employees { get; set; }

        [InverseProperty("WorkAreaResponsibleRole")]
        public Collection<WorkArea> WorkAreasResponsibles { get; set; }
        
        [InverseProperty("WorkAreaSupervisorRole")]
        public Collection<WorkArea> WorkAreasSupervisors { get; set; }
    }
}
