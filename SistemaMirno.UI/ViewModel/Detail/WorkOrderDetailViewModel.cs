using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    public class WorkOrderDetailViewModel : DetailViewModelBase, IWorkOrderDetailViewModel
    {
        private readonly IWorkOrderRepository _workOrderRepository;

        private WorkAreaWrapper _workArea;
        private WorkOrderWrapper _workOrder;
        private WorkUnitWrapper _newWorkUnit;
        private WorkUnitWrapper _selectedExistingWorkUnit;
        private WorkUnitWrapper _selectedRequisitionWorkUnit;
        private WorkOrderUnitWrapper _selectedWorkOrderUnit;

        private ProductWrapper _selectedProduct;
        private MaterialWrapper _selectedMaterial;
        private ColorWrapper _selectedColor;

        private Visibility _newWorkUnitGridVisibility;
        private Visibility _existingWorkUnitGridVisibility;
        private Visibility _removeWorkUnitVisibility;

        private string _quantity;
        private string _existingWorkUnitSearchText;
        private int _originWorkAreaId;

        private bool _canGetWorkUnitsFromRequisition;

        private PropertyGroupDescription _productName = new PropertyGroupDescription("Model.Product.Name");
        private PropertyGroupDescription _currentWorkAreaName = new PropertyGroupDescription("Model.CurrentWorkArea.Name");

        public WorkOrderDetailViewModel(
            IWorkOrderRepository workOrderRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Pedido", dialogCoordinator)
        {
            _workOrderRepository = workOrderRepository;

            Products = new ObservableCollection<ProductWrapper>();
            Materials = new ObservableCollection<MaterialWrapper>();
            Colors = new ObservableCollection<ColorWrapper>();

            Responsibles = new ObservableCollection<EmployeeWrapper>();
            Supervisors = new ObservableCollection<EmployeeWrapper>();

            WorkOrderUnits = new ObservableCollection<WorkOrderUnitWrapper>();
            ExistingWorkUnits = new ObservableCollection<WorkUnitWrapper>();
            RequisitionWorkUnits = new ObservableCollection<WorkUnitWrapper>();

            WorkOrderUnitsCollectionView = CollectionViewSource.GetDefaultView(WorkOrderUnits);
            WorkOrderUnitsCollectionView.GroupDescriptions.Add(_productName);
            ExistingWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(ExistingWorkUnits);
            ExistingWorkUnitsCollectionView.GroupDescriptions.Add(_productName);
            RequisitionWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(RequisitionWorkUnits);
            RequisitionWorkUnitsCollectionView.GroupDescriptions.Add(_currentWorkAreaName);
            RequisitionWorkUnitsCollectionView.GroupDescriptions.Add(_productName);

            AddWorkUnitCommand = new DelegateCommand<object>(OnAddWorkUnitExecute);
            AddNewWorkUnitCommand = new DelegateCommand(OnAddNewWorkUnitExecute, OnAddNewWorkUnitCanExecute);
            AddExistingWorkUnitCommand = new DelegateCommand(OnAddExistingWorkUnitExecute, OnAddExistingWorkUnitCanExecute);
            AddRequisitionWorkUnitCommand = new DelegateCommand(OnAddRequisitionWorkUnitExecute, OnAddRequisitionWorkUnitCanExecute);
            RemoveWorkUnitCommand = new DelegateCommand(OnRemoveWorkUnitExecute, OnRemoveWorkUnitCanExecute);
        }

        private bool OnAddExistingWorkUnitCanExecute()
        {
            return SelectedExistingWorkUnit != null;
        }

        private void OnAddExistingWorkUnitExecute()
        {
            while (SelectedExistingWorkUnit != null)
            {
                var workOrderUnit = new WorkOrderUnit
                {
                    WorkUnitId = SelectedExistingWorkUnit.Id,
                    WorkUnit = SelectedExistingWorkUnit.Model,
                };
                WorkOrder.Model.WorkOrderUnits.Add(workOrderUnit);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WorkOrderUnits.Add(new WorkOrderUnitWrapper(workOrderUnit));
                    ExistingWorkUnits.Remove(SelectedExistingWorkUnit);
                });
            }
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddRequisitionWorkUnitCanExecute()
        {
            return SelectedRequisitionWorkUnit != null;
        }

        private void OnAddRequisitionWorkUnitExecute()
        {
            while (SelectedRequisitionWorkUnit != null)
            {
                var workOrderUnit = new WorkOrderUnit
                {
                    WorkUnitId = SelectedRequisitionWorkUnit.Id,
                    WorkUnit = SelectedRequisitionWorkUnit.Model,
                };
                WorkOrder.Model.WorkOrderUnits.Add(workOrderUnit);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WorkOrderUnits.Add(new WorkOrderUnitWrapper(workOrderUnit));
                    RequisitionWorkUnits.Remove(SelectedRequisitionWorkUnit);
                });
            }
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveWorkUnitCanExecute()
        {
            return SelectedWorkOrderUnit != null;
        }

        private void OnRemoveWorkUnitExecute()
        {
            WorkOrder.Model.WorkOrderUnits.Remove(SelectedWorkOrderUnit.Model);
            Application.Current.Dispatcher.Invoke(() => WorkOrderUnits.Remove(SelectedWorkOrderUnit));
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
                    LatestResponsibleId = WorkOrder.ResponsibleEmployeeId,
                    LatestSupervisorId = WorkOrder.SupervisorEmployeeId,
                };

                var workOrderUnit = new WorkOrderUnit
                {
                    WorkUnit = workUnit,
                };

                WorkOrder.Model.WorkOrderUnits.Add(workOrderUnit);
                Application.Current.Dispatcher.Invoke(() => WorkOrderUnits.Add(new WorkOrderUnitWrapper(workOrderUnit)));
            }

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
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

        public bool CanGetWorkUnitsFromRequisition
        {
            get => _canGetWorkUnitsFromRequisition;

            set
            {
                _canGetWorkUnitsFromRequisition = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<WorkOrderUnitWrapper> WorkOrderUnits { get; }

        public ObservableCollection<WorkUnitWrapper> ExistingWorkUnits { get; }

        public ObservableCollection<WorkUnitWrapper> RequisitionWorkUnits { get; }

        public ObservableCollection<ProductWrapper> Products { get; }

        public ObservableCollection<MaterialWrapper> Materials { get; }

        public ObservableCollection<ColorWrapper> Colors { get; }

        public ObservableCollection<EmployeeWrapper> Responsibles { get; }

        public ObservableCollection<EmployeeWrapper> Supervisors { get; }

        public ICollectionView WorkOrderUnitsCollectionView { get; }

        public ICollectionView ExistingWorkUnitsCollectionView { get; }

        public ICollectionView RequisitionWorkUnitsCollectionView { get; }

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

                case "FromRequisitions":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        NewWorkUnitGridVisibility = Visibility.Collapsed;
                        ExistingWorkUnitGridVisibility = Visibility.Collapsed;
                        RequisitionWorkUnitGridVisibility = Visibility.Visible;
                    });
                    break;

                case "FromWorkAreas":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        NewWorkUnitGridVisibility = Visibility.Collapsed;
                        RequisitionWorkUnitGridVisibility = Visibility.Collapsed;
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

        public Visibility RequisitionWorkUnitGridVisibility
        {
            get => _existingWorkUnitGridVisibility;

            set
            {
                _existingWorkUnitGridVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility RequisitionWorkUnitsProgressVisibility
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

        public Visibility AddWorkAreaWorkUnitVisibility => IsNew ? Visibility.Visible : Visibility.Collapsed;

        public Visibility AddNewWorkUnitVisibility => IsNew && CanGetWorkUnitsFromRequisition ? Visibility.Visible : Visibility.Collapsed;

        public Visibility AddRequisitionWorkUnitVisibility => IsNew && CanGetWorkUnitsFromRequisition ? Visibility.Visible : Visibility.Collapsed;

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
                OnPropertyChanged(nameof(AddWorkAreaWorkUnitVisibility));
                OnPropertyChanged(nameof(AddRequisitionWorkUnitVisibility));
                OnPropertyChanged(nameof(AddNewWorkUnitVisibility));
                OnPropertyChanged(nameof(RemoveWorkUnitVisibility));
            }
        }

        public WorkAreaWrapper WorkArea
        {
            get => _workArea;

            set
            {
                _workArea = value;
                OnPropertyChanged();
            }
        }

        public WorkOrderWrapper WorkOrder
        {
            get => _workOrder;

            set
            {
                _workOrder = value;
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

        public WorkOrderUnitWrapper SelectedWorkOrderUnit
        {
            get => _selectedWorkOrderUnit;

            set
            {
                _selectedWorkOrderUnit = value;
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

        public WorkUnitWrapper SelectedRequisitionWorkUnit
        {
            get => _selectedRequisitionWorkUnit;

            set
            {
                _selectedRequisitionWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)AddRequisitionWorkUnitCommand).RaiseCanExecuteChanged();
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

        public string RequisitionWorkUnitSearchText
        {
            get => _existingWorkUnitSearchText;

            set
            {
                _existingWorkUnitSearchText = value;
                OnPropertyChanged();
                FilterRequisitionWorkUnitsCollectionView(value);
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

        private void FilterRequisitionWorkUnitsCollectionView(string search)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                RequisitionWorkUnitsProgressVisibility = Visibility.Visible;
                RequisitionWorkUnitsCollectionView.Filter = item =>
                {
                    WorkUnitWrapper vitem = item as WorkUnitWrapper;
                    return vitem != null && vitem.Model.Product.Name.ToLowerInvariant().Contains(search.ToLowerInvariant());
                };
                RequisitionWorkUnitsProgressVisibility = Visibility.Hidden;
            });
        }

        public ICommand AddWorkUnitCommand { get; }

        public ICommand AddNewWorkUnitCommand { get; }

        public ICommand AddExistingWorkUnitCommand { get; }

        public ICommand AddRequisitionWorkUnitCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _workOrderRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                WorkOrder = new WorkOrderWrapper(model);
                WorkOrder.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            foreach (var workOrderUnit in WorkOrder.Model.WorkOrderUnits)
            {
                Application.Current.Dispatcher.Invoke(() => WorkOrderUnits.Add(new WorkOrderUnitWrapper(workOrderUnit)));
            }

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            WorkOrder.CreationDateTime = DateTime.Now;

            foreach (var workOrderUnit in WorkOrder.Model.WorkOrderUnits)
            {
                workOrderUnit.Finished = false;
                workOrderUnit.WorkUnit.CurrentWorkAreaId = WorkArea.Id;
                workOrderUnit.WorkUnit.LatestResponsibleId = WorkOrder.ResponsibleEmployeeId;
                workOrderUnit.WorkUnit.LatestSupervisorId = WorkOrder.SupervisorEmployeeId;
            }

            if (IsNew)
            {
                await _workOrderRepository.AddAsync(WorkOrder.Model);
            }
            else
            {
                await _workOrderRepository.SaveAsync(WorkOrder.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = _originWorkAreaId,
                    ViewModel = nameof(WorkUnitViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(WorkOrder) && WorkOrderUnits.Count > 0;
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
                HasChanges = _workOrderRepository.HasChanges();
            }

            if (e.PropertyName == nameof(WorkOrder.HasErrors))
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
            Application.Current.Dispatcher.Invoke(() =>
            {
                NewWorkUnitGridVisibility = Visibility.Collapsed;
                ExistingWorkUnitGridVisibility = Visibility.Collapsed;
                ExistingWorkUnitsProgressVisibility = Visibility.Hidden;
                RequisitionWorkUnitGridVisibility = Visibility.Collapsed;
                RequisitionWorkUnitsProgressVisibility = Visibility.Hidden;
            });

            await LoadResponsibles();
            await LoadSupervisors();

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            await LoadMaterials();
            await LoadColors();
            await LoadProducts();

            await LoadExistingWorkUnits();
            if (CanGetWorkUnitsFromRequisition)
            {
                await LoadRequisitionWorkUnits();
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                WorkOrder = new WorkOrderWrapper();
                WorkOrder.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                WorkOrder.CreationDateTime = DateTime.Now;
                WorkOrder.Observations = string.Empty;
                WorkOrder.ResponsibleEmployeeId = 0;
                WorkOrder.SupervisorEmployeeId = 0;
                WorkOrder.DestinationWorkAreaId = WorkArea.Id;
                WorkOrder.OriginWorkAreaId = _originWorkAreaId;

                IsNew = true;
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
                NewWorkUnit.CurrentWorkAreaId = WorkArea.Id;
                NewWorkUnit.Model.CurrentWorkArea = WorkArea.Model;
                NewWorkUnit.Details = string.Empty;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadSupervisors()
        {
            var employees = await _workOrderRepository.GetEmployeesWithRoleIdAsync(WorkArea.SupervisorRoleId);

            foreach (var employee in employees)
            {
                Application.Current.Dispatcher.Invoke(() => Supervisors.Add(new EmployeeWrapper(employee)));
            }
        }

        private async Task LoadResponsibles()
        {
            var employees = await _workOrderRepository.GetEmployeesWithRoleIdAsync(WorkArea.ResponsibleRoleId);

            foreach (var employee in employees)
            {
                Application.Current.Dispatcher.Invoke(() => Responsibles.Add(new EmployeeWrapper(employee)));
            }
        }

        private async Task LoadRequisitionWorkUnits()
        {
            var workUnits = await _workOrderRepository.GetRequisitionWorkUnits();

            foreach (var workUnit in workUnits)
            {
                Application.Current.Dispatcher.Invoke(() => RequisitionWorkUnits.Add(new WorkUnitWrapper(workUnit)));
            }
        }

        private async Task LoadExistingWorkUnits()
        {
            var workUnits = await _workOrderRepository.GetExistingWorkUnits(WorkArea.Model.IncomingConnections);

            foreach (var workUnit in workUnits)
            {
                Application.Current.Dispatcher.Invoke(() => ExistingWorkUnits.Add(new WorkUnitWrapper(workUnit)));
            }
        }

        private async Task LoadProducts()
        {
            var products = await _workOrderRepository.GetAllProductsAsync();

            foreach (var product in products)
            {
                Application.Current.Dispatcher.Invoke(() => Products.Add(new ProductWrapper(product)));
            }
        }

        private async Task LoadColors()
        {
            var colors = await _workOrderRepository.GetAllColorsAsync();

            foreach (var color in colors)
            {
                Application.Current.Dispatcher.Invoke(() => Colors.Add(new ColorWrapper(color)));
            }
        }

        private async Task LoadMaterials()
        {
            var materials = await _workOrderRepository.GetAllMaterialsAsync();

            foreach (var material in materials)
            {
                Application.Current.Dispatcher.Invoke(() => Materials.Add(new MaterialWrapper(material)));
            }
        }

        public async Task CreateNewWorkOrder(int destinationWorkAreaId, int originWorkAreaId, IEnumerable<WorkUnitWrapper> workUnits )
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                NewWorkUnitGridVisibility = Visibility.Collapsed;
                ExistingWorkUnitGridVisibility = Visibility.Collapsed;
                ExistingWorkUnitsProgressVisibility = Visibility.Hidden;
                RequisitionWorkUnitGridVisibility = Visibility.Collapsed;
                RequisitionWorkUnitsProgressVisibility = Visibility.Hidden;
            });

            _originWorkAreaId = originWorkAreaId;

            var workArea = await _workOrderRepository.GetWorkAreaAsync(destinationWorkAreaId);

            Application.Current.Dispatcher.Invoke(() => WorkArea = new WorkAreaWrapper(workArea));

            if (workUnits != null)
            {
                foreach (var workUnit in workUnits)
                {
                    var workOrderUnit = new WorkOrderUnit()
                    {
                        WorkUnit = workUnit.Model,
                    };

                    Application.Current.Dispatcher.Invoke(() => WorkOrderUnits.Add(new WorkOrderUnitWrapper(workOrderUnit)));
                }
            }

            // Check if the current work area can receive units from requisitions (the first work area)
            CanGetWorkUnitsFromRequisition = workArea.IncomingConnections.Any(c => c.OriginWorkArea.IsFirst);

            await LoadAsync().ConfigureAwait(false);
        }
    }
}
