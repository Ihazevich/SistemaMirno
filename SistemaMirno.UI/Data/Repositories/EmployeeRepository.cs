using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private MirnoDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDataService"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public EmployeeRepository(MirnoDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.SingleAsync<Employee>(e => e.Id == id);
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
        public void Add(Employee model)
        {
            _context.Employees.Add(model);
        }

        public void Remove(Employee model)
        {
            _context.Employees.Remove(model);
        }
    }
}
