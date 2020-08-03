using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnitRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public WorkOrderRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<WorkOrder>> GetAllAsync()
        {
            return await _context.WorkOrders.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<WorkOrder> GetByIdAsync(int id)
        {
            return await _context.WorkOrders.SingleAsync<WorkOrder>(w => w.Id == id);
        }

        public async Task<IEnumerable<WorkOrder>> GetByAreaIdAsync(int areaId)
        {
            return await _context.WorkOrders.Where(w => w.WorkAreaId == areaId).ToListAsync();
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
        public void Add(WorkOrder model)
        {
            _context.WorkOrders.Add(model);
        }

        public void Remove(WorkOrder model)
        {
            _context.WorkOrders.Remove(model);
        }
    }
}
