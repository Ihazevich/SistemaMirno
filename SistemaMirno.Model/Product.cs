using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class Product : BaseModel
    {
        public Product()
        {
            WorkUnits = new Collection<WorkUnit>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int WholesalePrice { get; set; }

        [Required]
        public int ProductionPrice { get; set; }

        public Collection<WorkUnit> WorkUnits { get; set; }
    }
}