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
    public class AreaDataService : IAreaDataService
    {
        private Func<MirnoDbContext> _contextCreator;

        public AreaDataService(Func<MirnoDbContext> contextCreator)
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
    }
}
