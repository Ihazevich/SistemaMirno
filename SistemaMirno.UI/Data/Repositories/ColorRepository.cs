// <copyright file="ColorRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// A class representing the data repository of the color data.
    /// </summary>
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