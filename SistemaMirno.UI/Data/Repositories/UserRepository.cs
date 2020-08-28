using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.Data.Repositories
{
    public class UserRepository : GenericRepository<User, MirnoDbContext>, IUserRepository
    {
        public UserRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            try
            {
                return await Context.Users.SingleOrDefaultAsync(u => u.Username == username);
            }
            catch (InvalidOperationException ex)
            {
                // Throw if more than one user with the same username is found
                EventAggregator.GetEvent<ShowDialogEvent>().Publish( new ShowDialogEventArgs
                {
                    Message = "Mas de un usuario con el mismo nombre encontrado, contacte al Administrador del Sistema.",
                    Title = "Error",
                });
                return null;
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        public async Task<List<Role>> GetAllRolesFromEmployeeAsync(int id)
        {
            try
            {
                return await Context.Roles
                    .Where(r => r.Employees
                    .Any(e => e.Id == id))
                    .ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                return await Context.Employees.ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

    }
}
