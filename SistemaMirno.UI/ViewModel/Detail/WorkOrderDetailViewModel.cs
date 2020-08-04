using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Util;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class WorkOrderDetailViewModel : DetailViewModelBase, IWorkOrderDetailViewModel
    {
        private IWorkOrderRepository _workOrderRepository;
        private IWorkUnitRepository _workUnitRepository;
        private WorkOrderWrapper _workOrder;
        private WorkUnitWrapper _workUnit;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderDetailViewModel"/> class.
        /// </summary>
        /// <param name="workOrderRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public WorkOrderDetailViewModel(
            IWorkOrderRepository workOrderRepository,
            IWorkUnitRepository workUnitRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _workOrderRepository = workOrderRepository;
            _workUnitRepository = workUnitRepository;

            WorkUnit = new WorkUnitWrapper(new WorkUnit());
            WorkUnits = new ObservableCollection<WorkUnitWrapper>();

            Colors = new ObservableCollection<ColorWrapper>();
            Materials = new ObservableCollection<MaterialWrapper>();
            Products = new ObservableCollection<ProductWrapper>();

            Responsibles = new ObservableCollection<EmployeeWrapper>();
            Supervisors = new ObservableCollection<EmployeeWrapper>();

            AddWorkUnitToWorkOrderCommand = new DelegateCommand(OnAddWorkUnitExecute);
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public WorkOrderWrapper WorkOrder
        {
            get
            {
                return _workOrder;
            }

            set
            {
                _workOrder = value;
                OnPropertyChanged();
            }
        }

        public WorkUnitWrapper WorkUnit
        {
            get
            {
                return _workUnit;
            }

            set
            {
                _workUnit = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddWorkUnitToWorkOrderCommand { get; }

        public ObservableCollection<ProductWrapper> Products { get; set; }

        public ObservableCollection<MaterialWrapper> Materials { get; set; }

        public ObservableCollection<ColorWrapper> Colors { get; set; }

        public ObservableCollection<WorkUnitWrapper> WorkUnits { get; set; }

        public ObservableCollection<EmployeeWrapper> Responsibles { get; set; }

        public ObservableCollection<EmployeeWrapper> Supervisors { get; set; }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? areaId)
        {
            var workOrder = CreateNewWorkOrder();
            workOrder.WorkAreaId = areaId.Value;

            workOrder.WorkArea = await _workOrderRepository.GetWorkAreaAsync(areaId.Value);

            WorkOrder = new WorkOrderWrapper(workOrder);
            WorkOrder.PropertyChanged += WorkOrder_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            await LoadColorsAsync();
            await LoadMaterialsAsync();
            await LoadProductsAsync();

            await LoadResponsiblesAsync(WorkOrder.WorkArea.WorkAreaResponsibleRoleId.Value);
            await LoadSupervisorsAsync(WorkOrder.WorkArea.WorkAreaSupervisorRoleId.Value);
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            WorkOrder.StartTime = DateTime.Now;
            _workOrderRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(WorkOrder.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return WorkOrder != null && !WorkOrder.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _workOrderRepository.Remove(WorkOrder.Model);
            await _workOrderRepository.SaveAsync();
            RaiseDataModelDeletedEvent(WorkOrder.Model);
        }

        private void WorkOrder_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
            if (!HasChanges)
            {
                HasChanges = _workOrderRepository.HasChanges();
            }

            if (e.PropertyName == nameof(WorkOrder.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private WorkOrder CreateNewWorkOrder()
        {
            var workOrder = new WorkOrder();
            _workOrderRepository.Add(workOrder);
            return workOrder;
        }

        private async Task LoadColorsAsync()
        {
            var colors = await _workOrderRepository.GetColorsAsync();
            Colors.Clear();
            foreach (var color in colors)
            {
                Colors.Add(new ColorWrapper(color));
            }
        }

        private async Task LoadMaterialsAsync()
        {
            var materials = await _workOrderRepository.GetMaterialsAsync();
            Materials.Clear();
            foreach (var material in materials)
            {
                Materials.Add(new MaterialWrapper(material));
            }
        }

        private async Task LoadProductsAsync()
        {
            var products = await _workOrderRepository.GetProductsAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(new ProductWrapper(product));
            }
        }

        private async Task LoadResponsiblesAsync(int roleId)
        {
            var responsibles = await _workOrderRepository.GetEmployeesAsync(roleId);
            Responsibles.Clear();
            foreach (var responsible in responsibles)
            {
                Responsibles.Add(new EmployeeWrapper(responsible));
            }
        }

        private async Task LoadSupervisorsAsync(int roleId)
        {
            var supervisors = await _workOrderRepository.GetEmployeesAsync(roleId);
            Supervisors.Clear();
            foreach (var supervisor in supervisors)
            {
                Supervisors.Add(new EmployeeWrapper(supervisor));
            }
        }

        private void OnAddWorkUnitExecute()
        {
            WorkUnit.Product = Products.Where(p => p.Id == WorkUnit.ProductId).Single().Model;
            WorkUnit.Material = Materials.Where(m => m.Id == WorkUnit.MaterialId).Single().Model;
            WorkUnit.Color = Colors.Where(c => c.Id == WorkUnit.ColorId).Single().Model;

            var newWorkUnit = new WorkUnitWrapper(WorkUnit.Model);
            newWorkUnit.Quantity = WorkUnit.Quantity;
            for (int i = 0; i < newWorkUnit.Quantity; i++)
            {
                var workUnit = new WorkUnit {
                    WorkAreaId = WorkOrder.WorkAreaId,
                    ColorId = WorkUnit.ColorId,
                    MaterialId = WorkUnit.MaterialId,
                    ProductId = WorkUnit.ProductId,
                };

                WorkOrder.WorkUnits.Add(workUnit);
            }

            WorkUnits.Add(newWorkUnit);
            WorkUnit = new WorkUnitWrapper(new WorkUnit());
        }
    }
}
