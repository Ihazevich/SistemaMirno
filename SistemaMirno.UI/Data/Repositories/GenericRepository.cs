// <copyright file="GenericRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// A class representing a generic data repository.
    /// </summary>
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : ModelBase
        where TContext : DbContext
    {
        protected readonly TContext Context;

        protected GenericRepository(TContext context)
        {
            Context = context;
        }

        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public void Remove(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}