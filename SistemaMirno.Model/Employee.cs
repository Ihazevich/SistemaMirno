using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public class Employee : BaseModel
    {
        public Employee()
        {
            ResponsibleWorkOrders = new Collection<WorkOrder>();
            SupervisorWorkOrders = new Collection<WorkOrder>();
        }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        public int DocumentNumber { get; set; }

        public DateTime HiredDate { get; set; }

        public int EmployeeRoleId { get; set; }

        [InverseProperty("ResponsibleEmployee")]
        public virtual Collection<WorkOrder> ResponsibleWorkOrders { get; set; }

        [InverseProperty("SupervisorEmployee")]
        public virtual Collection<WorkOrder> SupervisorWorkOrders { get; set; }

        //TODO: Add SaleOrders
    }
}