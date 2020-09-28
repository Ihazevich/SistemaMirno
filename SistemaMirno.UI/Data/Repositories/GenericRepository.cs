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
    /// <summary>
    /// Represents the generic data repository.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TContext">The database context.</typeparam>
    public class GenericRepository<TEntity, TContext> : IDisposable, IGenericRepository<TEntity>
        where TEntity : ModelBase, new()
        where TContext : DbContext
    {
        private readonly Func<TContext> _dbCreator;
        private readonly DbSet<TEntity> _entities;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity, TContext}"/> class.
        /// </summary>
        /// <param name="contextCreator">The context creator.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        protected GenericRepository(Func<TContext> contextCreator, IEventAggregator eventAggregator)
        {
            _dbCreator = contextCreator;
            Context = _dbCreator();
            _entities = Context.Set<TEntity>();
            EventAggregator = eventAggregator;
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        protected TContext Context { get; }

        /// <summary>
        /// Gets the event aggregator.
        /// </summary>
        protected IEventAggregator EventAggregator { get; }

        /// <inheritdoc/>
        public Task<int> AddAsync(TEntity entity)
        {
            _entities.Add(entity);
            return SaveChangesAsync();
        }

        /// <inheritdoc/>
        public Task<int> AddRangeAsync(IList<TEntity> entities)
        {
            _entities.AddRange(entities);
            return SaveChangesAsync();
        }

        /// <inheritdoc/>
        public Task<int> DeleteAsync(int id, byte[] timeStamp)
        {
            Context.Entry(new TEntity() { Id = id, Timestamp = timeStamp }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }

        /// <inheritdoc/>
        public Task<int> DeleteAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            return SaveChangesAsync();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Context?.Dispose();
        }

        /// <inheritdoc/>
        public virtual Task<List<TEntity>> GetAllAsync() => _entities.ToListAsync();

        /// <inheritdoc/>
        public virtual Task<TEntity> GetByIdAsync(int? id) => _entities.FindAsync(id);

        /// <inheritdoc/>
        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        /// <inheritdoc/>
        public Task<int> SaveAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously saves the changes made to the database context.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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