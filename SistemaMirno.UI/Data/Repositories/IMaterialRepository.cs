using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public interface IMaterialRepository
    {
        /// <summary>
        /// Gets all the Materials from the database context.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<List<Material>> GetAllAsync();

        /// <summary>
        /// Gets a Production Area that matches the provided id.
        /// </summary>
        /// <param name="id">A <see cref="int"/> value representing the id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<Material> GetByIdAsync(int id);

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

        void Add(Material model);

        void Remove(Material model);
    }
}