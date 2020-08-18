// <copyright file="ClientRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// A class representing the data repository of the client data.
    /// </summary>
    public class ClientRepository : GenericRepository<Client, MirnoDbContext>, IClientRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ClientRepository(MirnoDbContext context)
            : base(context)
        {
        }
    }
}
