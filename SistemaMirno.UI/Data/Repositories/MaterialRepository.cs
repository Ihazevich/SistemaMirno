using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDataService"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public MaterialRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<Material>> GetAllAsync()
        {
            return await _context.Materials.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Material> GetByIdAsync(int id)
        {
            return await _context.Materials.SingleAsync<Material>(m => m.Id == id);
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
        public void Add(Material model)
        {
            _context.Materials.Add(model);
        }

        public void Remove(Material model)
        {
            _context.Materials.Remove(model);
        }
    }
}
