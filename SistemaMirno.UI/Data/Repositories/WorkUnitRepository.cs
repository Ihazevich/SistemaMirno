using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class WorkUnitRepository : GenericRepository<WorkUnit, MirnoDbContext>, IWorkUnitRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnitRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public WorkUnitRepository(MirnoDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<WorkUnit>> GetByAreaIdAsync(int areaId)
        {
            return await Context.WorkUnits.Where(w => w.WorkAreaId == areaId).ToListAsync();
        }

        public async Task<WorkArea> GetWorkAreaByIdAsync(int areaId)
        {
            return await Context.ProductionAreas.FindAsync(areaId);
        }
    }
}