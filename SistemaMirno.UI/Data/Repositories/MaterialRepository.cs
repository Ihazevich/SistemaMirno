using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data.Repositories
{
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