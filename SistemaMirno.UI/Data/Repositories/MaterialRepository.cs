// <copyright file="MaterialRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// A class representing the data repository of the product material data.
    /// </summary>
    public class MaterialRepository : GenericRepository<Material, MirnoDbContext>, IMaterialRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public MaterialRepository(MirnoDbContext context)
            : base(context)
        {
        }
    }
}