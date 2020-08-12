// <copyright file="UserRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Data.Entity;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// A class representing the data repository of the user data.
    /// </summary>
    public class UserRepository : GenericRepository<User, MirnoDbContext>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserRepository(MirnoDbContext context)
            : base(context)
        {
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await Context.Users.SingleOrDefaultAsync(u => u.Name == name);
        }
    }
}
