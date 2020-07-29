using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public class ColorDataService : IColorDataService
    {
        private Func<MirnoDbContext> _contextCreator;

        public ColorDataService(Func<MirnoDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<Color>> GetAllAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Colors.AsNoTracking().ToListAsync();
            }
        }
    }
}
