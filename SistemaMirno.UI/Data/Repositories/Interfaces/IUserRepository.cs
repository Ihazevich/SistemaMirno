using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);

        Task<List<Role>> GetAllRolesFromEmployeeAsync(int id);

        Task<List<Employee>> GetAllEmployeesAsync();
    }
}