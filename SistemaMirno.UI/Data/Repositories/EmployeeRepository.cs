// <copyright file="EmployeeRepository.cs" company="HazeLabs">
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
    /// Represents the data repository for the <see cref="Employee"/> model.
    /// </summary>
    public class EmployeeRepository : GenericRepository<Employee, MirnoDbContext>, IEmployeeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRepository"/> class.
        /// </summary>
        /// <param name="contextCreator">The context creator.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public EmployeeRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }

        /// <inheritdoc/>
        public Task<List<Branch>> GetAllBranchesAsync()
        {
            return Context.Branches.ToListAsync();
        }

        /// <inheritdoc/>
        public Task<List<Role>> GetAllRolesFromBranchAsync(int branchId)
        {
            return Context.Roles.Where(r => r.BranchId == branchId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<WorkOrderUnit>> GetThisMonthWorkOrderUnitsFromEmployeeAsync(int employeeId)
        {
            try
            {
                return await Context.WorkOrderUnits.Where(w =>
                    w.WorkOrder.ResponsibleEmployeeId == employeeId &&
                    w.WorkOrder.CreationDateTime.Year == DateTime.Today.Year &&
                    w.WorkOrder.CreationDateTime.Month == DateTime.Today.Month)
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

        /// <inheritdoc/>
        public async Task<List<WorkOrderUnit>> GetThisYearsWorkOrderUnitsFromEmployeeAsync(int employeeId)
        {
            try
            {
                return await Context.WorkOrderUnits.Where(w =>
                    w.WorkOrder.ResponsibleEmployeeId == employeeId &&
                    w.WorkOrder.CreationDateTime.Year == DateTime.Today.Year)
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
