// <copyright file="WorkUnitDetailViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class WorkUnitDetailViewModel : DetailViewModelBase, IWorkUnitDetailViewModels
    {
        private IWorkUnitRepository _workUnitRepository;
        private WorkUnitWrapper _workUnit;

        public WorkUnitDetailViewModel(
            IWorkUnitRepository workUnitRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Unidad de Trabajo", dialogCoordinator)
        {
            _workUnitRepository = workUnitRepository;

        }

        public override Task LoadAsync(int? id = null)
        {
            return Task.CompletedTask;
        }

        protected override bool OnSaveCanExecute()
        {
            return false;
        }
    }
}
