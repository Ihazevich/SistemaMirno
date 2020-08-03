using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class WorkUnit : BaseModel
    {
        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public int MaterialId { get; set; }

        public virtual Material Material { get; set; }

        [Required]
        public int ColorId { get; set; }

        public virtual Color Color { get; set; }

        [Required]
        public int WorkOrderId { get; set; }

        public virtual WorkOrder WorkOrder { get; set; }

        [Required]
        public int ProductionAreaId { get; set; }

        public virtual WorkArea ProductionArea { get; set; }
    }
}