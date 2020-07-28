using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public class WorkUnitDataService : IWorkUnitDataService
    {
        private Func<MirnoDbContext> _contextCreator;

        public WorkUnitDataService(Func<MirnoDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<WorkUnit>> GetWorkUnitsByAreaIdAsync(int areaId)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.WorkUnits.AsNoTracking()
                    .Where(w => w.ProductionAreaId == areaId)
                    .ToListAsync();
            }
        }

        public async Task<string> GetProductionAreaName(int areaId)
        {
            using (var ctx = _contextCreator())
            {
                var area = await ctx.ProductionAreas.AsNoTracking()
                    .Where(a => a.Id == areaId)
                    .SingleAsync();

                return area.Name;
            }
        }
    }
}
