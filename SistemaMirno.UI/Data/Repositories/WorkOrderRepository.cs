using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class WorkOrderRepository : GenericRepository<WorkOrder, MirnoDbContext>, IWorkOrderRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnitRepository"/> class.
        /// </summary>
        /// <param name="context">A <see cref="MirnoDbContext"/> instance representing the database context.</param>
        public WorkOrderRepository(MirnoDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<WorkOrder>> GetByAreaIdAsync(int areaId)
        {
            return await Context.WorkOrders.Where(w => w.WorkAreaId == areaId).ToListAsync();
        }

        public async Task<string> GetWorkAreaNameAsync(int areaId)
        {
            var area = await Context.ProductionAreas.Where(a => a.Id == areaId).SingleAsync();

            return area.Name;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Context.Set<Product>().ToListAsync();
        }

        public async Task<IEnumerable<Color>> GetColorsAsync()
        {
            return await Context.Set<Color>().ToListAsync();
        }

        public async Task<IEnumerable<Material>> GetMaterialsAsync()
        {
            return await Context.Set<Material>().ToListAsync();
        }

        public async Task<WorkArea> GetWorkAreaAsync(int id)
        {
            return await Context.Set<WorkArea>().FindAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(int roleId)
        {
            return await Context.Set<Employee>().Where(e => e.EmployeeRoleId == roleId).ToListAsync();
        }
    }
}
