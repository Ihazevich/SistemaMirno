using System.ComponentModel.DataAnnotations;

namespace SistemaMirno.Model
{
    public class ProductCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
