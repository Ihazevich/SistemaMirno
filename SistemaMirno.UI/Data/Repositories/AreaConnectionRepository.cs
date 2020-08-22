using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// A class representing the data repository of the work areas connection data.
    /// </summary>
    public class AreaConnectionRepository : GenericRepository<WorkAreaConnection, MirnoDbContext>, IAreaConnectionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaConnectionRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public AreaConnectionRepository(MirnoDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets the connections matching the provided work area Id.
        /// </summary>
        /// <param name="areaId">The work area id</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IEnumerable<WorkAreaConnection>> GetByAreaIdAsync(int areaId)
        {
            return await Context.AreaConnections.Where(c => c.WorkAreaId == areaId).ToListAsync();
        }

        /// <summary>
        /// Gets a collection of all work areas.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IEnumerable<WorkArea>> GetWorkAreasAsync()
        {
            return await Context.WorkAreas.ToListAsync();
        }

        /// <summary>
        /// Gets a work area that matches the provided id.
        /// </summary>
        /// <param name="id">The work area id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<WorkArea> GetWorkAreaByIdAsync(int id)
        {
            return await Context.WorkAreas.FindAsync(id);
        }
    }
}
