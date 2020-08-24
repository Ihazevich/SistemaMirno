using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;

namespace SistemaMirno.UI.Data.Repositories
{
    public class UserRepository : GenericRepository<User, MirnoDbContext>, IUserRepository
    {
        public UserRepository(MirnoDbContext context, IMessageDialogService dialogService, IEventAggregator eventAggregator)
            : base(context, dialogService, eventAggregator)
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

    }
}
