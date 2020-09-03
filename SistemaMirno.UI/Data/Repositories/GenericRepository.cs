// <copyright file="GenericRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.Data.Repositories
{
    public class GenericRepository<TEntity, TContext> : IDisposable, IGenericRepository<TEntity>
        where TEntity : ModelBase, new()
        where TContext : DbContext
    {
        private readonly Func<TContext> _dbCreator;
        private readonly DbSet<TEntity> _entities;

        protected GenericRepository(Func<TContext> contextCreator, IEventAggregator eventAggregator)
        {
            _dbCreator = contextCreator;
            Context = _dbCreator();
            _entities = Context.Set<TEntity>();
            EventAggregator = eventAggregator;
        }

        protected TContext Context { get; }

        protected IEventAggregator EventAggregator { get; }

        public Task<int> AddAsync(TEntity entity)
        {
            _entities.Add(entity);
            return SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(IList<TEntity> entities)
        {
            _entities.AddRange(entities);
            return SaveChangesAsync();
        }

        public Task<int> DeleteAsync(int id, byte[] timeStamp)
        {
            Context.Entry(new TEntity() { Id = id, Timestamp = timeStamp }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            return SaveChangesAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public virtual Task<List<TEntity>> GetAllAsync() => _entities.ToListAsync();

        public Task<TEntity> GetByIdAsync(int? id) => _entities.FindAsync(id);

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public Task<int> SaveAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return SaveChangesAsync();
        }

        internal async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error de concurrencia al intentar guardar a la base de datos. Ya fue modificado por otro usuario. \n {ex.Message}",
                        Title = "Error",
                    });
                return -1;
            }
            catch (DbUpdateException ex)
            {
                var details = ex.Message;
                if (ex.InnerException != null)
                {
                    details = ex.InnerException.Message;
                }

                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error al intentar guardar a la base de datos. Contacte al Administrador de Sistema.\n {details}",
                        Title = "Error",
                    });
                return -1;
            }
            catch (CommitFailedException ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error al ejecutar la transaccion con la base de datos. Contacte al Administrador de Sistema. \n {ex.Message}",
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
    }
}