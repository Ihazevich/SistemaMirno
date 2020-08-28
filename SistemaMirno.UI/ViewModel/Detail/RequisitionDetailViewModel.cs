using System;
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
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class RequisitionDetailViewModel : DetailViewModelBase, IRequisitionDetailViewModel
    {
        private readonly IRequisitionRepository _requisitionRepository;

        private RequisitionWrapper _requisition;
        private WorkUnitWrapper _newWorkUnit;
        private WorkUnitWrapper _selectedWorkUnit;
        private WorkUnitWrapper _selectedExistingWorkUnit;
        private ProductWrapper _selectedProduct;
        private MaterialWrapper _selectedMaterial;
        private ColorWrapper _selectedColor;

        private Visibility _newWorkUnitGridVisibility;
        private Visibility _existingWorkUnitGridVisibility;
        private Visibility _removeWorkUnitVisibility;

        private string _quantity;
        private string _existingWorkUnitSearchText;
        private bool _isForStock;
        private bool _hasTargetDate;

        private PropertyGroupDescription _productName = new PropertyGroupDescription("Model.Product.Name");

        public RequisitionDetailViewModel(
            IRequisitionRepository requisitionRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Pedido", dialogCoordinator)
        {
            _requisitionRepository = requisitionRepository;

            Clients = new ObservableCollection<ClientWrapper>();
            Products = new ObservableCollection<ProductWrapper>();
            Materials = new ObservableCollection<MaterialWrapper>();
            Colors = new ObservableCollection<ColorWrapper>();
            WorkUnits = new ObservableCollection<WorkUnitWrapper>();
            ExistingWorkUnits = new ObservableCollection<WorkUnitWrapper>();
            Priorities = new ObservableCollection<string>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Priorities.Add("Normal");
                Priorities.Add("Urgente");
            });

            WorkUnitsCollectionView = CollectionViewSource.GetDefaultView(WorkUnits);
            WorkUnitsCollectionView.GroupDescriptions.Add(_productName);
            ExistingWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(ExistingWorkUnits);
            ExistingWorkUnitsCollectionView.GroupDescriptions.Add(_productName);

            AddWorkUnitCommand = new DelegateCommand<object>(OnAddWorkUnitExecute);
            AddNewWorkUnitCommand = new DelegateCommand(OnAddNewWorkUnitExecute, OnAddNewWorkUnitCanExecute);
            AddExistingWorkUnitCommand = new DelegateCommand(OnAddExistingWorkUnitExecute, OnAddExistingWorkUnitCanExecute);
            RemoveWorkUnitCommand = new DelegateCommand(OnRemoveWorkUnitExecute, OnRemoveWorkUnitCanExecute);
        }
        
        private bool OnAddExistingWorkUnitCanExecute()
        {
            return SelectedExistingWorkUnit != null;
        }

        private void OnAddExistingWorkUnitExecute()
        {
            Requisition.Model.WorkUnits.Add(SelectedExistingWorkUnit.Model);
            Application.Current.Dispatcher.Invoke(() =>
            {
                WorkUnits.Add(SelectedExistingWorkUnit);
                ExistingWorkUnits.Remove(SelectedExistingWorkUnit);
            });
        }

        private bool OnRemoveWorkUnitCanExecute()
        {
            return SelectedWorkUnit != null;
        }

        private void OnRemoveWorkUnitExecute()
        {
            Requisition.Model.WorkUnits.Remove(SelectedWorkUnit.Model);
            Application.Current.Dispatcher.Invoke(() => WorkUnits.Remove(SelectedWorkUnit));
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddNewWorkUnitCanExecute()
        {
            return int.TryParse(Quantity, out _) && !NewWorkUnit.HasErrors;
        }

        private void OnAddNewWorkUnitExecute()
        {
            var quantity = int.Parse(Quantity);

            for (var i = 0; i < quantity; i++)
            {
                var workUnit = new WorkUnit
                {
                    ProductId = NewWorkUnit.ProductId,
                    MaterialId = NewWorkUnit.MaterialId,
                    ColorId = NewWorkUnit.ColorId,
                    Product = SelectedProduct.Model,
                    Material = SelectedMaterial.Model,
                    Color = SelectedColor.Model,
                    Delivered = NewWorkUnit.Delivered,
                    CreationDate = DateTime.Now,
                    TotalWorkTime = NewWorkUnit.TotalWorkTime,
                    Details = NewWorkUnit.Details,
                    CurrentWorkArea = NewWorkUnit.Model.CurrentWorkArea,
                    CurrentWorkAreaId = NewWorkUnit.CurrentWorkAreaId,
                    LatestResponsibleId = NewWorkUnit.LatestResponsibleId,
                    LatestSupervisorId = NewWorkUnit.LatestSupervisorId,
                };

                Requisition.Model.WorkUnits.Add(workUnit);
                Application.Current.Dispatcher.Invoke(() => WorkUnits.Add(new WorkUnitWrapper(workUnit)));
            }

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public bool ClientsEnabled => !IsForStock;

        public bool IsForStock
        {
            get => _isForStock;

            set
            {
                _isForStock = value;
                Requisition.IsForStock = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ClientsEnabled));
                OnPropertyChanged(nameof(AddExistingWorkUnitVisibility));
                OnPropertyChanged(nameof(AddNewWorkUnitVisibility));
            }
        }

        public string Quantity
        {
            get => _quantity;

            set
            {
                _quantity = int.TryParse(value, out _) ? value : string.Empty;
                OnPropertyChanged();
                ((DelegateCommand)AddNewWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<WorkUnitWrapper> WorkUnits { get; }

        public ObservableCollection<WorkUnitWrapper> ExistingWorkUnits { get; }

        public ObservableCollection<ClientWrapper> Clients { get; }

        public ObservableCollection<ProductWrapper> Products { get; }

        public ObservableCollection<MaterialWrapper> Materials { get; }

        public ObservableCollection<ColorWrapper> Colors { get; }

        public ObservableCollection<string> Priorities { get; }

        public ICollectionView WorkUnitsCollectionView { get; }

        public ICollectionView ExistingWorkUnitsCollectionView { get; }

        private void OnAddWorkUnitExecute(object obj)
        {
            switch (obj.ToString())
            {
                case "New":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        NewWorkUnitGridVisibility = Visibility.Visible;
                        ExistingWorkUnitGridVisibility = Visibility.Collapsed;
                    });
                    break;
                case "Existing":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        NewWorkUnitGridVisibility = Visibility.Collapsed;
                        ExistingWorkUnitGridVisibility = Visibility.Visible;
                    });
                    break;
            }
        }

        public Visibility NewWorkUnitGridVisibility
        {
            get => _newWorkUnitGridVisibility;

            set
            {
                _newWorkUnitGridVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility ExistingWorkUnitGridVisibility
        {
            get => _existingWorkUnitGridVisibility;

            set
            {
                _existingWorkUnitGridVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility ExistingWorkUnitsProgressVisibility
        {
            get => _existingWorkUnitGridVisibility;

            set
            {
                _existingWorkUnitGridVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility NewButtonsVisibility => IsNew ? Visibility.Visible : Visibility.Collapsed;

        public Visibility DetailButtonsVisibility => IsNew ? Visibility.Collapsed : Visibility.Visible;
        
        public Visibility AddExistingWorkUnitVisibility => IsForStock || !IsNew ? Visibility.Collapsed : Visibility.Visible;

        public Visibility AddNewWorkUnitVisibility => IsNew ? Visibility.Visible : Visibility.Collapsed;

        public Visibility RemoveWorkUnitVisibility => IsNew ? Visibility.Visible : Visibility.Collapsed;

        public override bool IsNew
        {
            get => base.IsNew;

            set
            {
                base.IsNew = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NewButtonsVisibility));
                OnPropertyChanged(nameof(DetailButtonsVisibility));
                OnPropertyChanged(nameof(AddExistingWorkUnitVisibility));
                OnPropertyChanged(nameof(AddNewWorkUnitVisibility));
                OnPropertyChanged(nameof(RemoveWorkUnitVisibility));
            }
        }

        public RequisitionWrapper Requisition
        {
            get => _requisition;

            set
            {
                _requisition = value;
                OnPropertyChanged();
            }
        }

        public WorkUnitWrapper NewWorkUnit
        {
            get => _newWorkUnit;

            set
            {
                _newWorkUnit = value;
                OnPropertyChanged();
            }
        }

        public WorkUnitWrapper SelectedWorkUnit
        {
            get => _selectedWorkUnit;

            set
            {
                _selectedWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkUnitWrapper SelectedExistingWorkUnit
        {
            get => _selectedExistingWorkUnit;

            set
            {
                _selectedExistingWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)AddExistingWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public ProductWrapper SelectedProduct
        {
            get => _selectedProduct;

            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public MaterialWrapper SelectedMaterial
        {
            get => _selectedMaterial;

            set
            {
                _selectedMaterial = value;
                OnPropertyChanged();
            }
        }

        public ColorWrapper SelectedColor
        {
            get => _selectedColor;

            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }

        public bool HasTargetDate
        {
            get => _hasTargetDate;

            set
            {
                _hasTargetDate = value;
                OnPropertyChanged();
            }
        }

        public string ExistingWorkUnitSearchText
        {
            get => _existingWorkUnitSearchText;

            set
            {
                _existingWorkUnitSearchText = value;
                OnPropertyChanged();
                FilterExistingWorkUnitsCollectionView(value);
            }
        }

        private void FilterExistingWorkUnitsCollectionView(string search)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ExistingWorkUnitsProgressVisibility = Visibility.Visible;
                ExistingWorkUnitsCollectionView.Filter = item =>
                {
                    WorkUnitWrapper vitem = item as WorkUnitWrapper;
                    return vitem != null && vitem.Model.Product.Name.ToLowerInvariant().Contains(search.ToLowerInvariant());
                };
                ExistingWorkUnitsProgressVisibility = Visibility.Hidden;
            });
        }

        public ICommand AddWorkUnitCommand { get; }

        public ICommand AddNewWorkUnitCommand { get; }

        public ICommand AddExistingWorkUnitCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _requisitionRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Requisition = new RequisitionWrapper(model);
                Requisition.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            foreach (var workUnit in Requisition.Model.WorkUnits)
            {
                Application.Current.Dispatcher.Invoke(() => WorkUnits.Add(new WorkUnitWrapper(workUnit)));
            }

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            Requisition.RequestedDate = DateTime.Now;

            if (IsForStock)
            {
                Requisition.ClientId = null;
            }

            if (!HasTargetDate)
            {
                Requisition.TargetDate = null;
            }

            if (IsNew)
            {
                await _requisitionRepository.AddAsync(Requisition.Model);
            }
            else
            {
                await _requisitionRepository.SaveAsync(Requisition.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Requisition) && WorkUnits.Count > 0;
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            await _requisitionRepository.DeleteAsync(Requisition.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _requisitionRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Requisition.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void NewWorkUnit_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewWorkUnit.HasErrors))
            {
                ((DelegateCommand)AddNewWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            var firstWorkArea = await _requisitionRepository.GetFirstWorkAreaAsync();

            if (firstWorkArea == null)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Title = "Advertencia",
                        Message = "Area de Pedidos no existe, notifique al Administrador de Sistema.",
                    });
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                NewWorkUnitGridVisibility = Visibility.Collapsed;
                ExistingWorkUnitGridVisibility = Visibility.Collapsed;
                ExistingWorkUnitsProgressVisibility = Visibility.Hidden;
            });

            await LoadClients();

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            await LoadMaterials();
            await LoadColors();
            await LoadProducts();
            await LoadExistingWorkUnits();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Requisition = new RequisitionWrapper();
                Requisition.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                Requisition.RequestedDate = DateTime.Now;
                Requisition.Fulfilled = false;

                IsNew = true;
                IsForStock = false;
                HasTargetDate = false;
                Quantity = string.Empty;

                NewWorkUnit = new WorkUnitWrapper();
                NewWorkUnit.PropertyChanged += NewWorkUnit_PropertyChanged;
                ((DelegateCommand)AddNewWorkUnitCommand).RaiseCanExecuteChanged();

                NewWorkUnit.ProductId = 0;
                NewWorkUnit.MaterialId = 0;
                NewWorkUnit.ColorId = 0;
                NewWorkUnit.Delivered = false;
                NewWorkUnit.CreationDate = DateTime.Now;
                NewWorkUnit.TotalWorkTime = 0;
                NewWorkUnit.LatestResponsibleId = null;
                NewWorkUnit.LatestSupervisorId = null;
                NewWorkUnit.CurrentWorkAreaId = firstWorkArea.Id;
                NewWorkUnit.Model.CurrentWorkArea = firstWorkArea;
                NewWorkUnit.Details = string.Empty;
            });
            
            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadExistingWorkUnits()
        {
            var workUnits = await _requisitionRepository.GetAllUnassignedWorkUnitsAsync();
            
            foreach (var workUnit in workUnits)
            {
                Application.Current.Dispatcher.Invoke(() => ExistingWorkUnits.Add(new WorkUnitWrapper(workUnit)));
            }
        }

        private async Task LoadProducts()
        {
            var products = await _requisitionRepository.GetAllProductsAsync();

            foreach (var product in products)
            {
                Application.Current.Dispatcher.Invoke(() => Products.Add(new ProductWrapper(product)));
            }
        }

        private async Task LoadColors()
        {
            var colors = await _requisitionRepository.GetAllColorsAsync();

            foreach (var color in colors)
            {
                Application.Current.Dispatcher.Invoke(() => Colors.Add(new ColorWrapper(color)));
            }
        }

        private async Task LoadMaterials()
        {
            var materials = await _requisitionRepository.GetAllMaterialsAsync();

            foreach (var material in materials)
            {
                Application.Current.Dispatcher.Invoke(() => Materials.Add(new MaterialWrapper(material)));
            }
        }

        private async Task LoadClients()
        {
            var clients = await _requisitionRepository.GetAllClientsAsync();

            foreach (var client in clients)
            {
                Application.Current.Dispatcher.Invoke(() => Clients.Add(new ClientWrapper(client)));
            }
        }
    }
}
