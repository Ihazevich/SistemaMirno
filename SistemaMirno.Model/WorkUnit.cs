using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class WorkUnit : BaseModel
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public int MaterialId { get; set; }

        public Material Material { get; set; }

        [Required]
        public int ColorId { get; set; }

        public Color Color { get; set; }

        [Required]
        public int WorkOrderId { get; set; }

        public WorkOrder WorkOrder { get; set; }

        [Required]
        public int ProductionAreaId { get; set; }

        public ProductionArea ProductionArea { get; set; }
    }
}