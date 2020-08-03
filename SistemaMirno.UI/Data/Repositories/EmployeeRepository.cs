using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee, MirnoDbContext>, IEmployeeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDataService"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public EmployeeRepository(MirnoDbContext context)
            : base(context)
        {
        }
    }
}
