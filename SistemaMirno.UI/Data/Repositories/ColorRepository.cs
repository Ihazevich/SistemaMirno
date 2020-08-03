using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class ColorRepository : GenericRepository<Color, MirnoDbContext>, IColorRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public ColorRepository(MirnoDbContext context)
            : base(context)
        {
        }
    }
}