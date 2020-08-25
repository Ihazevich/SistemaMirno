// <copyright file="GenericRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;

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
        private TContext _db;
        private Func<TContext> _dbCreator;
        private readonly IEventAggregator _eventAggregator;

        protected GenericRepository(Func<TContext> contextCreator, IEventAggregator eventAggregator)
        {
            _dbCreator = contextCreator;
            _db = _dbCreator();
            _table = _db.Set<TEntity>();
            _eventAggregator = eventAggregator;
        }

        protected IEventAggregator EventAggregator => _eventAggregator;

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
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Error de concurrencia al intentar guardar a la base de datos. Ya fue modificado por otro usuario.",
                        Title = "Error",
                    });
                return -1;
            }
            catch (DbUpdateException ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Error al intentar guardar a la base de datos. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return -1;
            }
            catch (CommitFailedException ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Error al ejecutar la transaccion con la base de datos. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return -1;
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return -1;
            }
        }

        public async Task<TEntity> GetByIdAsync(int? id) => await _table.FindAsync(id);

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

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }
    }
}