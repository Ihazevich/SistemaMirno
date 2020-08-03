using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDataService"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public ColorRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<Color>> GetAllAsync()
        {
            return await _context.Colors.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Color> GetByIdAsync(int id)
        {
            return await _context.Colors.SingleAsync<Color>(c => c.Id == id);
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
        public void Add(Color model)
        {
            _context.Colors.Add(model);
        }

        public void Remove(Color model)
        {
            _context.Colors.Remove(model);
        }
    }
}