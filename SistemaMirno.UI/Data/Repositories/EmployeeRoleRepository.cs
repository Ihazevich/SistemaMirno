// <copyright file="EmployeeRoleRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// A class representing the data repository of the employee's role data.
    /// </summary>
    public class EmployeeRoleRepository : GenericRepository<EmployeeRole,MirnoDbContext>, IEmployeeRoleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRoleRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public EmployeeRoleRepository(MirnoDbContext context)
            : base(context)
        {
        }
    }
}
