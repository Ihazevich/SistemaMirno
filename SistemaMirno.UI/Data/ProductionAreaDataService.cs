using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public class ProductionAreaDataService : IProductionAreaDataService
    {
        private Func<MirnoDbContext> _contextCreator;

        public ProductionAreaDataService(Func<MirnoDbContext> contextCreator)
        {
            _contextCreator = contextCreator;        
        }

        public async Task<List<ProductionArea>> GetAllAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.ProductionAreas.AsNoTracking().ToListAsync();
            }
        }

        public async Task SaveAsync(ProductionArea area)
        {
            using (var ctx = _contextCreator())
            {
                ctx.ProductionAreas.Attach(area);
                ctx.Entry(area).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
