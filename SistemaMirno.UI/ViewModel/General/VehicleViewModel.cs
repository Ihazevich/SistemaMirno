using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class VehicleViewModel : ViewModelBase
    {
        private IVehicleRepository _vehicleRepository;
        private VehicleWrapper _selectedVehicle;

        public VehicleViewModel(
            IVehicleRepository vehicleRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Vehiculos", dialogCoordinator)
        {
            _vehicleRepository = vehicleRepository;

            Vehicles = new ObservableCollection<VehicleWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedVehicle.Id,
                    ViewModel = nameof(VehicleDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedVehicle != null;
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(VehicleDetailViewModel),
                });
        }

        public ObservableCollection<VehicleWrapper> Vehicles { get; }

        public VehicleWrapper SelectedVehicle
        {
            get
            {
                return _selectedVehicle;
            }

            set
            {
                OnPropertyChanged();
                _selectedVehicle = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Vehicles.Clear();

            var vehicles = await _vehicleRepository.GetAllAsync();

            foreach (var vehicle in vehicles)
            {
                Application.Current.Dispatcher.Invoke(() => Vehicles.Add(new VehicleWrapper(vehicle)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }
    }
}
