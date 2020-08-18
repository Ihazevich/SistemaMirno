using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class Sale : BaseModel
    {
        public int RequisitionId { get; set; }
        
        public virtual Requisition Requisition { get; set; }
        
        public int ResponsibleId { get; set; }
        
        public virtual Employee Responsible { get; set; }
        
        public DateTime DeliveryDate { get; set; }
        
        public DateTime EstimatedDeliveryDate { get; set; }
        
        public int Discount { get; set; }
        
        public int IVA { get; set; }

        public int SaleType { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
