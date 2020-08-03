using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory, MirnoDbContext>, IProductCategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public ProductCategoryRepository(MirnoDbContext context)
            : base(context)
        {
        }
    }
}