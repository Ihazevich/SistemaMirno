// <copyright file="ProductRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// A class representing the data repository of the product data.
    /// </summary>
    public class ProductRepository : GenericRepository<Product, MirnoDbContext>, IProductRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public ProductRepository(MirnoDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Returns the id of a product category matching with the provided name.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<int> GetProductCategoryIdAsync(string categoryName)
        {
            var category = await Context.ProductCategories.SingleOrDefaultAsync(c => c.Name == categoryName);

            if (category != null)
            {
                return category.Id;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Checks if the database already has a model with the same name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<bool> CheckForDuplicatesAsync(string name)
        {
            var result = await Context.Products.Where(p => p.Name == name).ToListAsync();

            return result.Count > 0;
        }
    }
}