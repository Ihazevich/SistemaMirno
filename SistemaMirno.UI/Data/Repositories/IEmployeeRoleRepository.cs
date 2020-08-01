using SistemaMirno.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Repositories
{
    public interface IEmployeeRoleRepository
    {
        /// <summary>
        /// Gets all the EmployeeRoles from the database context.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<List<EmployeeRole>> GetAllAsync();

        /// <summary>
        /// Gets a EmployeeRole that matches the provided id.
        /// </summary>
        /// <param name="id">A <see cref="int"/> value representing the id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<EmployeeRole> GetByIdAsync(int id);

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

        void Add(EmployeeRole model);

        void Remove(EmployeeRole model);
    }
}