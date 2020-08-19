using System;
using System.Collections.Generic;

namespace SistemaMirno.Model
{
    public class Sale : BaseModel
    {
        public int BranchId { get; set; }

        public virtual Branch Branch { get; set; }

        public int RequisitionId { get; set; }

        public virtual Requisition Requisition { get; set; }

        public int ResponsibleId { get; set; }

        public virtual Employee Responsible { get; set; }

        public DateTime DeliveryDate { get; set; }

        public DateTime EstimatedDeliveryDate { get; set; }

        public int Total { get; set; }

        public int Discount { get; set; }

        public int IVA { get; set; }

        public int SaleTypeId { get; set; }

        public virtual SaleType SaleType { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
