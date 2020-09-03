// <copyright file="ColorRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

namespace SistemaMirno.UI.Data.Repositories
{
    public class ColorRepository : GenericRepository<Color, MirnoDbContext>, IColorRepository
    {
        public ColorRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }
    }
}
