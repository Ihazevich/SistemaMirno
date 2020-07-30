using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// Class representing the Production Area model repository.
    /// </summary>
    public class ProductionAreaRepository : IProductionAreaRepository
    {
        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionAreaRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public ProductionAreaRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<ProductionArea>> GetAllAsync()
        {
            return await _context.ProductionAreas.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ProductionArea> GetByIdAsync(int id)
        {
            return await _context.ProductionAreas.SingleAsync<ProductionArea>(p => p.Id == id);
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
        public void Add(ProductionArea productionArea)
        {
            _context.ProductionAreas.Add(productionArea);
        }

        public void Remove(ProductionArea model)
        {
            _context.ProductionAreas.Remove(model);
        }
    }
}
