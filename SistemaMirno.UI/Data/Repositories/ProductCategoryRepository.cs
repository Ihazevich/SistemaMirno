using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public ProductCategoryRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<ProductCategory>> GetAllAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await _context.ProductCategories.SingleAsync<ProductCategory>(c => c.Id == id);
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
        public void Add(ProductCategory model)
        {
            _context.ProductCategories.Add(model);
        }

        public void Remove(ProductCategory model)
        {
            _context.ProductCategories.Remove(model);
        }
    }
}
