using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class AreaConnectionRepository : GenericRepository<AreaConnection, MirnoDbContext>, IAreaConnectionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaConnectionRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public AreaConnectionRepository(MirnoDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<AreaConnection>> GetByAreaIdAsync(int areaId)
        {
            return await Context.AreaConnections.Where(c => c.WorkAreaId == areaId).ToListAsync();
        }

        public async Task<IEnumerable<WorkArea>> GetWorkAreasAsync()
        {
            return await Context.ProductionAreas.ToListAsync();
        }

        public async Task<WorkArea> GetWorkAreaByIdAsync(int id)
        {
            return await Context.ProductionAreas.FindAsync(id);
        }
    }
}
