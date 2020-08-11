// <copyright file="ProductCategoryRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// A class representing the data repository of the product category data.
    /// </summary>
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