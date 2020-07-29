using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// Interface for the <see cref="ProductionAreaRepository"/> class.
    /// </summary>
    public interface IProductionAreaRepository
    {
        /// <summary>
        /// Gets all the Production Areas from the database context.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<List<ProductionArea>> GetAllAsync();

        /// <summary>
        /// Saves all changes made to the database context.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task SaveAsync();
    }
}