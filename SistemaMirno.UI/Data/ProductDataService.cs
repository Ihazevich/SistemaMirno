using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public class ProductDataService : IDataService
    {
        public IEnumerable<BaseModel> GetAll()
        {
            // TODO: Connect to real database
            yield return new Product { Name = "Cama Oydis", Category = "Cama", Price = 1000000, WholesalePrice = 850000 };
            yield return new Product { Name = "Mesa de luz Oydis", Category = "Mesa de luz", Price = 400000, WholesalePrice = 350000 };
            yield return new Product { Name = "Comoda Oydis de 4 cajones", Category = "Comoda", Price = 2000000, WholesalePrice = 1200000 };
        }
    }
}
