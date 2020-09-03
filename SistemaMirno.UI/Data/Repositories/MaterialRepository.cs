// <copyright file="MaterialRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

namespace SistemaMirno.UI.Data.Repositories
{
    public class MaterialRepository : GenericRepository<Material, MirnoDbContext>, IMaterialRepository
    {
        public MaterialRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }
    }
}
