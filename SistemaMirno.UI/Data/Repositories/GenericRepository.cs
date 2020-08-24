// <copyright file="GenericRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// A class representing a generic data repository.
    /// </summary>
    public class GenericRepository<TEntity, TContext> : IDisposable, IGenericRepository<TEntity>
        where TEntity : ModelBase, new()
        where TContext : DbContext
    {
        private readonly DbSet<TEntity> _table;
        private readonly TContext _db;

        protected GenericRepository(TContext context)
        {
            _db = context;
            _table = _db.Set<TEntity>();
        }

        protected TContext Context => _db;

        public void Dispose()
        {
            _db?.Dispose();
        }

        internal async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw;
            }
            catch (CommitFailedException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TEntity> GetOneAsync(int? id) => await _table.FindAsync(id);

        public virtual async Task<List<TEntity>> GetAllAsync() => await _table.ToListAsync();

        public async Task<int> AddAsync(TEntity entity)
        {
            _table.Add(entity);
            return await SaveChangesAsync();
        }

        public async Task<int> AddRangeAsync(IList<TEntity> entities)
        {
            _table.AddRange(entities);
            return await SaveChangesAsync();
        }

        public async Task<int> SaveAsync(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            return await SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id, byte[] timeStamp)
        {
            _db.Entry(new TEntity() {Id = id, Timestamp = timeStamp}).State = EntityState.Deleted;
            return await SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Deleted;
            return await SaveChangesAsync();
        }
    }
}