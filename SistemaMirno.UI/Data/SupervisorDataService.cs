using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public class SupervisorDataService : ISupervisorDataService
    {
        private Func<MirnoDbContext> _contextCreator;

        public SupervisorDataService(Func<MirnoDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<Supervisor>> GetAllAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Supervisors.AsNoTracking().ToListAsync();
            }
        }
    }
}
