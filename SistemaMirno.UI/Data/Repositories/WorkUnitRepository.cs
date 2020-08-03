using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class WorkUnitRepository : IWorkUnitRepository
    {
        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnitRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public WorkUnitRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<WorkUnit>> GetAllAsync()
        {
            return await _context.WorkUnits.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<WorkUnit> GetByIdAsync(int id)
        {
            return await _context.WorkUnits.SingleAsync<WorkUnit>(w => w.Id == id);
        }

        public async Task<IEnumerable<WorkUnit>> GetByAreaIdAsync(int areaId)
        {
            return await _context.WorkUnits.Where(w => w.ProductionAreaId == areaId).ToListAsync();
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
        public void Add(WorkUnit model)
        {
            _context.WorkUnits.Add(model);
        }

        public void Remove(WorkUnit model)
        {
            _context.WorkUnits.Remove(model);
        }
    }
}