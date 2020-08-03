using SistemaMirno.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Repositories
{
    public interface IWorkUnitRepository
    {
        /// <summary>
        /// Gets all the Production Areas from the database context.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<List<WorkUnit>> GetAllAsync();

        /// <summary>
        /// Gets a Production Area that matches the provided id.
        /// </summary>
        /// <param name="id">A <see cref="int"/> value representing the id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<WorkUnit> GetByIdAsync(int id);

        /// <summary>
        /// Gets all the Work Units that match the provided Work Area id.
        /// </summary>
        /// <param name="id">A <see cref="int"/> value representing the id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<WorkUnit>> GetByAreaIdAsync(int areaId);

        Task<string> GetWorkAreaNameAsync(int areaId);

        /// <summary>
        /// Saves all changes made to the database context.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task SaveAsync();

        /// <summary>
        /// Checks if the database context has any changes.
        /// </summary>
        /// <returns>True or false.</returns>
        bool HasChanges();

        void Add(WorkUnit model);

        void Remove(WorkUnit model);
    }
}