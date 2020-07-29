using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public class ProductDataService : IProductDataService
    {
        private Func<MirnoDbContext> _contextCreator;

        public ProductDataService(Func<MirnoDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Products.AsNoTracking().ToListAsync();
            }
        }

        public async Task SaveAsync(Product product)
        {
            using (var ctx = _contextCreator())
            {
                ctx.Products.Attach(product);
                ctx.Entry(product).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
