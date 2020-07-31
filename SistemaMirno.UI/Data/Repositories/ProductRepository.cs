using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public ProductRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.SingleAsync<Product>(p => p.Id == id);
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
        public void Add(Product model)
        {
            _context.Products.Add(model);
        }

        public void Remove(Product model)
        {
            _context.Products.Remove(model);
        }
    }
}
