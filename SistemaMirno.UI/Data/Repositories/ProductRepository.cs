// <copyright file="ProductRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// Represents the data repository for the <see cref="Product"/> model.
    /// </summary>
    public class ProductRepository : GenericRepository<Product, MirnoDbContext>, IProductRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="contextCreator">The context creator.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public ProductRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }

        /// <inheritdoc/>
        public async Task<List<ProductCategory>> GetAllProductCategoriesAsync()
        {
            try
            {
                return await Context.ProductCategories.ToListAsync();
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<ProductCategory> GetProductCategoryByNameAsync(string productCategoryName)
        {
            try
            {
                return await Context.ProductCategories.SingleOrDefaultAsync(c =>
                    string.Equals(c.Name, productCategoryName, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> CheckForDuplicatesAsync(string productName)
        {
            try
            {
                return await Task.Run(() => Context.Products.Select(p => p.Name.ToLower()).Contains(productName.ToLower()));
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return true;
            }
        }
    }
}
