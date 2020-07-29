﻿using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Data
{
    public class ResponsibleDataService : IResponsibleDataService
    {
        private Func<MirnoDbContext> _contextCreator;

        public ResponsibleDataService(Func<MirnoDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<List<Responsible>> GetAllAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Responsibles.AsNoTracking().ToListAsync();
            }
        }
    }
}