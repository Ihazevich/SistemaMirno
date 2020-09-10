using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class DeliveryOrderDetailViewModel : DetailViewModelBase
    {
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private DeliveryOrderWrapper _deliveryOrder;
        private Sale _selectedSale;
        private WorkUnit _selectedSaleWorkUnit;
        private WorkUnit _selectedDeliveryWorkUnit;
        private string _saleClientFilter;
        private readonly PropertyGroupDescription _clientFullName = new PropertyGroupDescription("Sale.Client.FullName");
        private readonly PropertyGroupDescription _description = new PropertyGroupDescription("Description");

        public DeliveryOrderDetailViewModel(
            IDeliveryOrderRepository deliveryOrderRepository,
            IEventAggregator eventAggregator, 
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalle de Orden de Entrega", dialogCoordinator)
        {
            _deliveryOrderRepository = deliveryOrderRepository;

            Sales = new ObservableCollection<Sale>();
            SaleWorkUnits = new ObservableCollection<WorkUnit>();
            DeliveryWorkUnits = new ObservableCollection<WorkUnit>();
            Vehicles = new ObservableCollection<Vehicle>();
            Responsibles = new ObservableCollection<Employee>();

            SalesCollectionView = CollectionViewSource.GetDefaultView(Sales);
            SaleWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(SaleWorkUnits);
            DeliveryWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(DeliveryWorkUnits);

            SaleWorkUnitsCollectionView.GroupDescriptions.Add(_clientFullName);
            SaleWorkUnitsCollectionView.GroupDescriptions.Add(_description);
            DeliveryWorkUnitsCollectionView.GroupDescriptions.Add(_clientFullName);
            DeliveryWorkUnitsCollectionView.GroupDescriptions.Add(_description);

            AddWorkUnitCommand = new DelegateCommand(AddWorkUnitExecute, AddWorkUnitCanExecute);
            RemoveWorkUnitCommand = new DelegateCommand(RemoveWorkUnitExecute, RemoveWorkUnitCanExecute);
        }

        public void FilterSales()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SalesCollectionView.Filter = item =>
                {
                    return item is Sale vitem && vitem.Client.FullName.ToLowerInvariant()
                        .Contains(_saleClientFilter.ToLowerInvariant());
                };
            });
        }

        public string SaleClientFilter
        {
            get => _saleClientFilter;

            set
            {
                _saleClientFilter = value;
                OnPropertyChanged();
                FilterSales();
            }
        }

        public void FilterSaleWorkUnits()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SaleWorkUnitsCollectionView.Filter = item =>
                {
                    if (SelectedSale == null)
                    {
                        return false;
                    }

                    return item is WorkUnit vitem &&
                           vitem.SaleId == SelectedSale.Id;
                };
            });
        }

        public Sale SelectedSale
        {
            get => _selectedSale;

            set
            {
                _selectedSale = value;
                OnPropertyChanged();
                FilterSaleWorkUnits();
            }
        }

        private bool RemoveWorkUnitCanExecute()
        {
            return SelectedDeliveryWorkUnit != null;
        }

        private void RemoveWorkUnitExecute()
        {
            while (SelectedDeliveryWorkUnit != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DeliveryWorkUnits.Add(SelectedDeliveryWorkUnit);
                    SaleWorkUnits.Remove(SelectedDeliveryWorkUnit);
                });
            }

            ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
        }

        private bool AddWorkUnitCanExecute()
        {
            return SelectedSaleWorkUnit != null;
        }

        private void AddWorkUnitExecute()
        {
            while (SelectedSaleWorkUnit != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DeliveryWorkUnits.Add(SelectedSaleWorkUnit);
                    SaleWorkUnits.Remove(SelectedSaleWorkUnit);
                });
            }

            ((DelegateCommand)AddWorkUnitCommand).RaiseCanExecuteChanged();
        }

        public WorkUnit SelectedSaleWorkUnit
        {
            get => _selectedSaleWorkUnit;

            set
            {
                _selectedSaleWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)AddWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkUnit SelectedDeliveryWorkUnit
        {
            get => _selectedDeliveryWorkUnit;

            set
            {
                _selectedDeliveryWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddWorkUnitCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

        public ICollectionView SalesCollectionView { get; }

        public ICollectionView SaleWorkUnitsCollectionView { get; }

        public ICollectionView DeliveryWorkUnitsCollectionView { get; }

        public ObservableCollection<Sale> Sales { get; }

        public ObservableCollection<WorkUnit> SaleWorkUnits { get; }

        public ObservableCollection<WorkUnit> DeliveryWorkUnits { get; }

        public ObservableCollection<Vehicle> Vehicles { get; }

        public ObservableCollection<Employee> Responsibles { get; }

        public DeliveryOrderWrapper DeliveryOrder
        {
            get => _deliveryOrder;

            set
            {
                _deliveryOrder = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _deliveryOrderRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                DeliveryOrder = new DeliveryOrderWrapper(model);
                DeliveryOrder.PropertyChanged += Model_PropertyChanged;
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
                await _deliveryOrderRepository.AddAsync(DeliveryOrder.Model);
            }
            else
            {
                await _deliveryOrderRepository.SaveAsync(DeliveryOrder.Model);
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
            return OnSaveCanExecute(DeliveryOrder);
        }

        protected override void OnCancelExecute()
        {
            base.OnCancelExecute();
            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(true);
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _deliveryOrderRepository.HasChanges();
            }

            if (e.PropertyName == nameof(DeliveryOrder.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadResponsiblesAsync();
            await LoadVehiclesAsync();

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            await LoadSalesAsync();
            await LoadWorkUnitsAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                DeliveryOrder = new DeliveryOrderWrapper();
                DeliveryOrder.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                DeliveryOrder.Date = DateTime.Today;
                DeliveryOrder.ResponsibleId = 0;
                DeliveryOrder.VehicleId = 0;
                DeliveryOrder.KmBefore = 0;
                DeliveryOrder.KmAfter = 0;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadVehiclesAsync()
        {
            var vehicles = await _deliveryOrderRepository.GetAllVehiclesAsync();

            foreach (var vehicle in vehicles)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Vehicles.Add(vehicle);
                });
            }
        }

        private async Task LoadResponsiblesAsync()
        {
            var responsibles = await _deliveryOrderRepository.GetAllLogisticResponsiblesAsync();

            foreach (var responsible in responsibles)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Responsibles.Add(responsible);
                });
            }
        }

        private async Task LoadSalesAsync()
        {
            var sales = await _deliveryOrderRepository.GetAllNonDeliveredSalesAsync();

            foreach (var sale in sales)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Sales.Add(sale);
                });
            }
        }

        private async Task LoadWorkUnitsAsync()
        {
            foreach (var sale in Sales)
            {
                foreach (var workUnit in sale.WorkUnits.Where(w => w.Delivered == false))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SaleWorkUnits.Add(workUnit);
                    });
                }
            }
        }
    }
}
