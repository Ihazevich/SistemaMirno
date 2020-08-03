using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class ProductCategory : BaseModel
    {
        public ProductCategory()
        {
            Products = new Collection<Product>();
        }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public Collection<Product> Products { get; set; }
    }
}