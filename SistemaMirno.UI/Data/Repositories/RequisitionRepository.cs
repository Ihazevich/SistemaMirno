// <copyright file="RequisitionRepository.cs" company="HazeLabs">
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
    /// Represents the data repository for the <see cref="Requisition"/> model.
    /// </summary>
    public class RequisitionRepository : GenericRepository<Requisition, MirnoDbContext>, IRequisitionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequisitionRepository"/> class.
        /// </summary>
        /// <param name="contextCreator">The context creator.</param>
        /// <param name="eventAggregator">The event aggregtor.</param>
        public RequisitionRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }

        /// <inheritdoc/>
        public async Task<List<Client>> GetAllClientsAsync()
        {
            try
            {
                return await Context.Clients.ToListAsync();
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                return await Context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Color>> GetAllColorsAsync()
        {
            try
            {
                return await Context.Colors.ToListAsync();
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Material>> GetAllMaterialsAsync()
        {
            try
            {
                return await Context.Materials.ToListAsync();
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<WorkArea> GetFirstWorkAreaAsync()
        {
            try
            {
                return await Context.WorkAreas.SingleAsync(w => w.IsFirst);
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<WorkUnit>> GetAllUnassignedWorkUnitsAsync()
        {
            try
            {
                return await Context.WorkUnits.Where(w => w.Delivered == false && (w.RequisitionId == null || w.Requisition.ClientId == null))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                        Title = "Error",
                    });
                return null;
            }
        }
    }
}
