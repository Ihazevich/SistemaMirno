using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    public partial class IntermediateWorkUnit : ModelBase
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public int IntermediateProductId { get; set; }

        [ForeignKey(nameof(IntermediateProductId))]
        public virtual IntermediateProduct IntermediateProduct { get; set; }
    }
}
