using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{

    /// <summary>
    /// Class representing the Production Area model repository.
    /// </summary>
    public class WorkAreaRepository : GenericRepository<WorkArea, MirnoDbContext>, IWorkAreaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public WorkAreaRepository(MirnoDbContext context)
            : base(context)
        {
        }
    }
}