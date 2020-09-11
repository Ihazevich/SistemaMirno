using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class SaleDetailViewModel : DetailViewModelBase
    {
        private readonly ISaleRepository _saleRepository;
        private SaleWrapper _sale;
        private WorkUnit _selectedSaleWorkUnit;
        private WorkUnit _selectedExistingWorkUnit;
        private Client _selectedClient;

        private string _workUnitProductSearchText;
        private string _workUnitMaterialSearchText;
        private string _workUnitColorSearchText;

        private readonly PropertyGroupDescription _description = new PropertyGroupDescription("Description");

        public SaleDetailViewModel(
            ISaleRepository saleRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Venta", dialogCoordinator)
        {
            _saleRepository = saleRepository;

            Branches = new ObservableCollection<Branch>();
            Clients = new ObservableCollection<Client>();
            Responsibles = new ObservableCollection<Employee>();
            ExistingWorkUnits = new ObservableCollection<WorkUnit>();
            SaleWorkUnits = new ObservableCollection<WorkUnit>();

            ExistingWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(ExistingWorkUnits);
            SaleWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(SaleWorkUnits);

            ExistingWorkUnitsCollectionView.GroupDescriptions.Add(_description);
            SaleWorkUnitsCollectionView.GroupDescriptions.Add(_description);

            AddWorkUnitCommand = new DelegateCommand(OnAddWorkUnitExecute, OnAddWorkUnitCanExecute);
            RemoveWorkUnitCommand = new DelegateCommand(OnRemoveWorkUnitExecute, OnRemoveWorkUnitCanExecute);
        }

        private bool OnAddWorkUnitCanExecute()
        {
            return SelectedExistingWorkUnit != null && Sale.ClientId > 0;
        }

        private void OnAddWorkUnitExecute()
        {
            while (SelectedExistingWorkUnit != null)
            {
                if (SelectedClient.IsWholesaler)
                {
                    Sale.Subtotal += SelectedExistingWorkUnit.Product.WholesalerPrice;
                }
                else if (SelectedClient.IsRetail)
                {
                    Sale.Subtotal += SelectedExistingWorkUnit.Product.RetailPrice;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    SaleWorkUnits.Add(SelectedExistingWorkUnit);
                    ExistingWorkUnits.Remove(SelectedExistingWorkUnit);
                    OnPropertyChanged(nameof(ExistingWorkUnitsCollectionView));
                });
            }

            ((DelegateCommand)AddWorkUnitCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveWorkUnitCanExecute()
        {
            return SelectedSaleWorkUnit != null;
        }

        private void OnRemoveWorkUnitExecute()
        {
            while (SelectedSaleWorkUnit != null)
            {
                if (SelectedClient.IsWholesaler)
                {
                    Sale.Subtotal -= SelectedSaleWorkUnit.Product.WholesalerPrice;
                }
                else if (SelectedClient.IsRetail)
                {
                    Sale.Subtotal -= SelectedSaleWorkUnit.Product.RetailPrice;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ExistingWorkUnits.Add(SelectedSaleWorkUnit);
                    SaleWorkUnits.Remove(SelectedSaleWorkUnit);
                    OnPropertyChanged(nameof(ExistingWorkUnitsCollectionView));
                });
            }

            ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
        }

        public Visibility RetailPriceVisibility => SelectedClient.IsRetail ? Visibility.Visible : Visibility.Collapsed;

        public Visibility WholesalerPriceVisibility => SelectedClient.IsWholesaler ? Visibility.Visible : Visibility.Collapsed;

        public ICommand AddWorkUnitCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

        public ICollectionView ExistingWorkUnitsCollectionView { get; }

        public ICollectionView SaleWorkUnitsCollectionView { get; }

        public ObservableCollection<Branch> Branches { get; }

        public ObservableCollection<Client> Clients { get; }

        public ObservableCollection<Employee> Responsibles { get; }

        public ObservableCollection<WorkUnit> ExistingWorkUnits { get; }

        public ObservableCollection<WorkUnit> SaleWorkUnits { get; }

        public SaleWrapper Sale
        {
            get => _sale;

            set
            {
                _sale = value;
                OnPropertyChanged();
            }
        }

        public WorkUnit SelectedSaleWorkUnit
        {
            get => _selectedSaleWorkUnit;

            set
            {
                _selectedSaleWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkUnit SelectedExistingWorkUnit
        {
            get => _selectedExistingWorkUnit;

            set
            {
                _selectedExistingWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)AddWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;

            set
            {
                _selectedClient = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RetailPriceVisibility));
                OnPropertyChanged(nameof(WholesalerPriceVisibility));

                Sale.Subtotal = 0;
                foreach (var workUnit in SaleWorkUnits)
                {
                    if (SelectedClient.IsWholesaler)
                    {
                        Sale.Subtotal += workUnit.Product.WholesalerPrice;
                    }
                    else if (SelectedClient.IsRetail)
                    {
                        Sale.Subtotal += workUnit.Product.RetailPrice;
                    }
                }
            }
        }

        public string WorkUnitProductSearchText
        {
            get => _workUnitProductSearchText;

            set
            {
                _workUnitProductSearchText = value;
                OnPropertyChanged();
                FilterExistingWorkUnits();
            }
        }

        public string WorkUnitMaterialSearchText
        {
            get => _workUnitMaterialSearchText;

            set
            {
                _workUnitMaterialSearchText = value;
                OnPropertyChanged();
                FilterExistingWorkUnits();
            }
        }

        public string WorkUnitColorSearchText
        {
            get => _workUnitColorSearchText;

            set
            {
                _workUnitColorSearchText = value;
                OnPropertyChanged();
                FilterExistingWorkUnits();
            }
        }

        private void FilterExistingWorkUnits()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ExistingWorkUnitsCollectionView.Filter = item =>
                {
                    if (string.IsNullOrWhiteSpace(WorkUnitColorSearchText) &&
                        string.IsNullOrWhiteSpace(WorkUnitMaterialSearchText) &&
                        string.IsNullOrWhiteSpace(WorkUnitProductSearchText))
                    {
                        return false;
                    }

                    return item is WorkUnit vitem &&
                           (vitem.Description.ToLowerInvariant()
                                .Contains(WorkUnitProductSearchText.ToLowerInvariant()) &&
                            vitem.Material.Name.ToLowerInvariant()
                                .Contains(WorkUnitMaterialSearchText.ToLowerInvariant()) &&
                            vitem.Color.Name.ToLowerInvariant().Contains(WorkUnitColorSearchText.ToLowerInvariant()));
                };
            });
        }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _saleRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Sale = new SaleWrapper(model);
                Sale.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            foreach (var workUnit in SaleWorkUnits)
            {
                workUnit.Sold = true;
                Sale.Model.WorkUnits.Add(workUnit);
            }

            Sale.InvoiceId = null;

            if (IsNew)
            {
                await _saleRepository.AddAsync(Sale.Model);
            }

            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(true);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Sale);
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
                HasChanges = _saleRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Sale.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            await Task.Run(async () =>
            {
                await LoadBranchesAsync();
                await LoadClientsAsync();
                await LoadResponsiblesAsync().ConfigureAwait(false);
            });

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            await Task.Run(LoadWorkUnitAsync);

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                Sale = new SaleWrapper();
                Sale.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                Sale.Date = DateTime.Today;
                Sale.ClientId = 0;
                Sale.ResponsibleId = 0;
                Sale.Discount = 0;
                Sale.Tax = 0;
                Sale.Subtotal = 0;
                Sale.Total = 0;
                Sale.DeliveryFee = 0;
                Sale.HasInvoice = false; // This already sets the invoiceId to 0
                Sale.BranchId = SessionInfo.Branch.Id;

                WorkUnitProductSearchText = string.Empty;
                WorkUnitMaterialSearchText = string.Empty;
                WorkUnitColorSearchText = string.Empty;
            });

            FilterExistingWorkUnits();

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadBranchesAsync()
        {
            var branches = await _saleRepository.GetAllBranchesAsync();

            foreach (var branch in branches)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Branches.Add(branch);
                });
            }
        }

        private async Task LoadClientsAsync()
        {
            var clients = await _saleRepository.GetAllClientsAsync();

            foreach (var client in clients)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clients.Add(client);
                });
            }
        }

        private async Task LoadResponsiblesAsync()
        {
            var responsibles = await _saleRepository.GetAllSalesResponsiblesAsync();

            foreach (var responsible in responsibles)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Responsibles.Add(responsible);
                });
            }
        }

        private async Task LoadWorkUnitAsync()
        {
            var workUnits = await _saleRepository.GetAllWorkUnitsAsync();

            foreach (var workUnit in workUnits)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ExistingWorkUnits.Add(workUnit);
                });
            }
        }
    }
}
