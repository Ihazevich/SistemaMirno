using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// Class representing the Production Area model repository.
    /// </summary>
    public class WorkAreaRepository : IWorkAreaRepository
    {
        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public WorkAreaRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<WorkArea>> GetAllAsync()
        {
            return await _context.ProductionAreas.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<WorkArea> GetByIdAsync(int id)
        {
            return await _context.ProductionAreas.SingleAsync<WorkArea>(p => p.Id == id);
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
        public void Add(WorkArea productionArea)
        {
            _context.ProductionAreas.Add(productionArea);
        }

        public void Remove(WorkArea model)
        {
            _context.ProductionAreas.Remove(model);
        }
    }
}