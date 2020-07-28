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
    public class MaterialDataService : IMaterialDataService
    {
        private Func<MirnoDbContext> _contextCreator;

        public MaterialDataService(Func<MirnoDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<Material>> GetAllAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Materials.AsNoTracking().ToListAsync();
            }
        }
    }
}
