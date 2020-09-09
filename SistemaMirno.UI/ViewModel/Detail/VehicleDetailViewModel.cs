using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class VehicleDetailViewModel : DetailViewModelBase
    {
        private IVehicleRepository _vehicleRepository;
        private VehicleWrapper _vehicle;

        public VehicleDetailViewModel(
            IVehicleRepository vehicleRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Vehiculo", dialogCoordinator)
        {
            _vehicleRepository = vehicleRepository;
        }

        public VehicleWrapper Vehicle
        {
            get => _vehicle;

            set
            {
                _vehicle = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _vehicleRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Vehicle = new VehicleWrapper(model);
                Vehicle.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            if (IsNew)
            {
                await _vehicleRepository.AddAsync(Vehicle.Model);
            }
            else
            {
                await _vehicleRepository.SaveAsync(Vehicle.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ColorViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Vehicle);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            base.OnDeleteExecute();
            await _vehicleRepository.DeleteAsync(Vehicle.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ColorViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            base.OnCancelExecute();
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ColorViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _vehicleRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Vehicle.HasErrors))
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

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                Vehicle = new VehicleWrapper();
                Vehicle.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                Vehicle.VehicleModel = string.Empty;
                Vehicle.Year = string.Empty;
                Vehicle.DinatranExpiration = DateTime.Today;
                Vehicle.PatentExpiration = DateTime.Today;
                Vehicle.FireExtinguisherExpiration = DateTime.Today;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }
    }
}