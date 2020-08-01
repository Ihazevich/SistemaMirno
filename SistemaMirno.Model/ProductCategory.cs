using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class ProductCategory
    {
        public ProductCategory()
        {
            Products = new Collection<Product>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public Collection<Product> Products { get; set; }
    }
}