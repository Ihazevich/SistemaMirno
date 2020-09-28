// <copyright file="WorkAreaRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// Represents the data repository for the <see cref="WorkArea"/> model.
    /// </summary>
    public class WorkAreaRepository : GenericRepository<WorkArea, MirnoDbContext>, IWorkAreaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaRepository"/> class.
        /// </summary>
        /// <param name="contextCreator">The context creator.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public WorkAreaRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }

        /// <inheritdoc/>
        public async Task<List<Branch>> GetAllBranchesAsync()
        {
            try
            {
                return await Context.Branches.ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<WorkArea>> GetAllWorkAreasAsync()
        {
            try
            {
                return await Context.WorkAreas.ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Role>> GetAllRolesAsync()
        {
            try
            {
                return await Context.Roles.ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<WorkAreaConnection>> GetAllWorkAreaConnectionsFromWorkAreaAsync(int id)
        {
            try
            {
                return await Context.WorkAreaConnections.Where(c => c.OriginWorkAreaId == id).ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return new List<WorkAreaConnection>();
            }
        }

        /// <inheritdoc/>
        public async Task<bool> CheckIfFirstExistsAsync(int id)
        {
            try
            {
                return await Context.WorkAreas.AnyAsync(w => w.IsFirst && w.BranchId == id);
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return true;
            }
        }

        /// <inheritdoc/>
        public async Task<List<WorkArea>> GetAllWorkAreasFromBranchAsync(int id)
        {
            try
            {
                return await Context.WorkAreas.Where(w => w.BranchId == id).ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> CheckIfLastExistsAsync(int id)
        {
            try
            {
                return await Context.WorkAreas.AnyAsync(w => w.IsLast && w.BranchId == id);
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return true;
            }
        }
    }
}
