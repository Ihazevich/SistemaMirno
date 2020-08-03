using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
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
