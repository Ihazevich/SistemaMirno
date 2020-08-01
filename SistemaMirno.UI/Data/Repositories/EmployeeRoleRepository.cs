using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Repositories
{
    public class EmployeeRoleRepository : IEmployeeRoleRepository
    {
        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRoleRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public EmployeeRoleRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<EmployeeRole>> GetAllAsync()
        {
            return await _context.EmployeeRoles.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<EmployeeRole> GetByIdAsync(int id)
        {
            return await _context.EmployeeRoles.SingleAsync<EmployeeRole>(r => r.Id == id);
        }

        /// <inheritdoc/>
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        /// <inheritdoc/>
        public void Add(EmployeeRole model)
        {
            _context.EmployeeRoles.Add(model);
        }

        public void Remove(EmployeeRole model)
        {
            _context.EmployeeRoles.Remove(model);
        }
    }
}
