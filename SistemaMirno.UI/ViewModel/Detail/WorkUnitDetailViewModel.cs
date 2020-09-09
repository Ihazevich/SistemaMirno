// <copyright file="WorkUnitDetailViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class WorkUnitDetailViewModel : DetailViewModelBase
    {
        private readonly IWorkUnitRepository _workUnitRepository;
        private WorkUnitWrapper _workUnit;

        public WorkUnitDetailViewModel(
            IWorkUnitRepository workUnitRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Unidad de Trabajo", dialogCoordinator)
        {
            _workUnitRepository = workUnitRepository;
        }

        public WorkUnitWrapper WorkUnit
        {
            get => _workUnit;

            set
            {
                _workUnit = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _workUnitRepository.GetByIdAsync(id);

            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WorkUnit = new WorkUnitWrapper(model);
                    WorkUnit.PropertyChanged += Model_PropertyChanged;
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                });

            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = e.Message,
                        Title = "Error",
                    });
                throw;
            }
            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            if (IsNew)
            {
                await _workUnitRepository.AddAsync(WorkUnit.Model);
            }
            else
            {
                await _workUnitRepository.SaveAsync(WorkUnit.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = WorkUnit.CurrentWorkAreaId,
                    ViewModel = nameof(WorkUnitViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return true;
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            base.OnDeleteExecute();
            await _workUnitRepository.DeleteAsync(WorkUnit.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = WorkUnit.CurrentWorkAreaId,
                    ViewModel = nameof(WorkUnitViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            base.OnCancelExecute();
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = WorkUnit.CurrentWorkAreaId,
                    ViewModel = nameof(WorkUnitViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _workUnitRepository.HasChanges();
            }

            if (e.PropertyName == nameof(WorkUnit.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            await base.LoadDetailAsync().ConfigureAwait(false);
        }
    }
}
