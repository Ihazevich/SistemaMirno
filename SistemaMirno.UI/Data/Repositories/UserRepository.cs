using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using SistemaMirno.UI.View.Services;

namespace SistemaMirno.UI.Data.Repositories
{
    public class UserRepository : GenericRepository<User, MirnoDbContext>, IUserRepository
    {
        public UserRepository(MirnoDbContext context, IMessageDialogService dialogService) 
            : base(context, dialogService)
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
                DialogService.ShowOkDialog(
                    "Mas de un usuario con el mismo nombre encontrado, contacte al Administrador del Sistema.",
                    "Error");
                throw;
            }
            catch (Exception e)
            {
                DialogService.ShowOkDialog(
                    $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    "Error");
                throw;
            }
        }

    }
}
