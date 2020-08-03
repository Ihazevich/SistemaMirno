using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class AreaConnectionRepository : IAreaConnectionRepository
    {

        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaConnectionRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public AreaConnectionRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<AreaConnection>> GetAllAsync()
        {
            return await _context.AreaConnections.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<AreaConnection> GetByIdAsync(int id)
        {
            return await _context.AreaConnections.SingleAsync<AreaConnection>(c => c.Id == id);
        }

        public async Task<IEnumerable<AreaConnection>> GetByAreaIdAsync(int areaId)
        {
            return await _context.AreaConnections.Where(c => c.FromWorkAreaId == areaId).ToListAsync();
        }

        public async Task<string> GetWorkAreaNameAsync(int areaId)
        {
            var area = await _context.ProductionAreas.Where(a => a.Id == areaId).SingleAsync();

            return area.Name;
        }

        /// <inheritdoc/>
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        /// <inheritdoc/>
        public void Add(AreaConnection model)
        {
            _context.AreaConnections.Add(model);
        }

        public void Remove(AreaConnection model)
        {
            _context.AreaConnections.Remove(model);
        }
    }
}
