using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Util;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using jsreport.Client;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Reports;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class WorkOrderDetailViewModel : DetailViewModelBase, IWorkOrderDetailViewModel
    {
        private IWorkOrderRepository _workOrderRepository;
        private WorkOrderWrapper _workOrder;
        private WorkUnitWrapper _workUnit;
        private bool _isNewOrder = true;
        private int _originAreaId;
        private int _destinationAreaId;

        private PropertyGroupDescription _clientName = new PropertyGroupDescription("Client.Name");
        private PropertyGroupDescription _colorName = new PropertyGroupDescription("Color.Name");
        private PropertyGroupDescription _materialName = new PropertyGroupDescription("Material.Name");
        private PropertyGroupDescription _productName = new PropertyGroupDescription("Product.Name");
        private int _workUnitQuantity;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderDetailViewModel"/> class.
        /// </summary>
        /// <param name="workOrderRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public WorkOrderDetailViewModel(
            IWorkOrderRepository workOrderRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _workOrderRepository = workOrderRepository;

            WorkUnit = new WorkUnitWrapper(new WorkUnit());
            WorkUnits = new ObservableCollection<WorkUnitWrapper>();

            WorkUnitCollection = CollectionViewSource.GetDefaultView(WorkUnits);

            Colors = new ObservableCollection<ColorWrapper>();
            Materials = new ObservableCollection<MaterialWrapper>();
            Products = new ObservableCollection<ProductWrapper>();

            Responsibles = new ObservableCollection<EmployeeWrapper>();
            Supervisors = new ObservableCollection<EmployeeWrapper>();

            AddWorkUnitToWorkOrderCommand = new DelegateCommand(OnAddWorkUnitExecute);

            FilterByClientCommand = new DelegateCommand<object>(OnFilterByClientExecute);
            FilterByColorCommand = new DelegateCommand<object>(OnFilterByColorExecute);
            FilterByMaterialCommand = new DelegateCommand<object>(OnFilterByMaterialExecute);
            FilterByProductCommand = new DelegateCommand<object>(OnFilterByProductExecute);
        }

        public bool IsNewOrder
        {
            get => _isNewOrder;

            set
            {
                _isNewOrder = value;
                OnPropertyChanged();
            }
        }

        public int WorkUnitQuantity
        {
            get => _workUnitQuantity;

            set
            {
                _workUnitQuantity = value;
                OnPropertyChanged();
            }
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

        public ICommand FilterByClientCommand { get; }

        public ICommand FilterByColorCommand { get; }

        public ICommand FilterByMaterialCommand { get; }

        public ICommand FilterByProductCommand { get; }

        public ObservableCollection<ProductWrapper> Products { get; set; }

        public ObservableCollection<MaterialWrapper> Materials { get; set; }

        public ObservableCollection<ColorWrapper> Colors { get; set; }

        public ObservableCollection<WorkUnitWrapper> WorkUnits { get; set; }

        public ICollectionView WorkUnitCollection { get; set; }

        public ObservableCollection<EmployeeWrapper> Responsibles { get; set; }

        public ObservableCollection<EmployeeWrapper> Supervisors { get; set; }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? areaId)
        {
            await LoadColorsAsync();
            await LoadMaterialsAsync();
            await LoadProductsAsync();
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            WorkOrder.StartTime = DateTime.Now;
            _workOrderRepository.Save();
            HasChanges = false;
            RaiseDataModelSavedEvent(WorkOrder.Model);

            SendToPrinter(CreateWorkOrderReport());

            ExitView();
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
            ExitView();
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
            // Create a new Wrapper for the WorkUnit Model.
            var newWorkUnitWrapper = new WorkUnitWrapper(WorkUnit.Model);

            // Create an ammount of Work Units equal to the specified quantity.
            for (int i = 0; i < WorkUnitQuantity; i++)
            {
                var newWorkUnit = new WorkUnit {
                    WorkAreaId = WorkOrder.DestinationWorkAreaId,
                    ColorId = WorkUnit.ColorId,
                    MaterialId = WorkUnit.MaterialId,
                    ProductId = WorkUnit.ProductId,
                };

                // Attach the Work Unit to a new Work Order Unit.
                var newWorkOrderUnit = new WorkOrderUnit();
                newWorkOrderUnit.WorkUnit = newWorkUnit;

                // Add the Work Unit to the Work Order
                WorkOrder.WorkOrderUnits.Add(newWorkOrderUnit);

                // Set the model for the Work Unit, add extra details to it.
                WorkUnit.Model = newWorkUnit;
                WorkUnit.Product = Products.Where(p => p.Id == WorkUnit.ProductId).Single().Model;
                WorkUnit.Material = Materials.Where(m => m.Id == WorkUnit.MaterialId).Single().Model;
                WorkUnit.Color = Colors.Where(c => c.Id == WorkUnit.ColorId).Single().Model;

                // Add the Work Unit to the Observable Collection to display it on the view datagrid.
                WorkUnits.Add(WorkUnit);
            }

            // After processing, reset the WorkUnit and the quantity.
            WorkUnit = new WorkUnitWrapper(new WorkUnit());
            WorkUnitQuantity = 0;
        }

        public async void CreateNewWorkOrder(int destinationAreaId, int originAreaId, ICollection<WorkUnitWrapper> workUnits = null)
        {
            // Create a new work order and add it to the repository
            var workOrder = CreateNewWorkOrder();

            // Set the entering and leaving areas
            workOrder.DestinationWorkAreaId = destinationAreaId;
            workOrder.OriginWorkAreaId = originAreaId;

            // Also save the id's in private variables to prevent data loss if the model gets deleted
            _destinationAreaId = destinationAreaId;
            _originAreaId = originAreaId;

            workOrder.DestinationWorkArea = await _workOrderRepository.GetWorkAreaAsync(destinationAreaId);
            workOrder.OriginWorkArea = await _workOrderRepository.GetWorkAreaAsync(originAreaId);

            // Create a new wrapper with the model and attach an event handler
            WorkOrder = new WorkOrderWrapper(workOrder);
            WorkOrder.PropertyChanged += WorkOrder_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            // If the order is a move order, it will have a collection of work units to move
            // if the collection exists, create the corresponding workOrder units and attach them
            // to the model.
            if (workUnits != null)
            {
                foreach (var workUnit in workUnits)
                {
                    // Set the Work Area of the Work Unit as the destination Work Area.
                    workUnit.WorkAreaId = _destinationAreaId;

                    // Attach this Work Unit to a new Work Order Unit.
                    var newWorkOrderUnit = new WorkOrderUnit();
                    newWorkOrderUnit.WorkUnit = workUnit.Model;

                    // Add the Work Order Unit to the Work Order.
                    WorkOrder.WorkOrderUnits.Add(newWorkOrderUnit);

                    // Add the Work Unit to the Observable Collection for display on the view Datagrid.
                    WorkUnits.Add(workUnit);
                }

                // Since the Work Order has a collection of Work Units, its not a new order.
                _isNewOrder = false;
            }

            // Check if Responsibles and Supervisors for this area exist.
            // If they exist, Load Responsibles and Supervisors for the destination Work Area.
            if (WorkOrder.DestinationWorkArea.WorkAreaResponsibleRoleId.HasValue)
            {
                await LoadResponsiblesAsync(WorkOrder.DestinationWorkArea.WorkAreaResponsibleRoleId.Value);
            }

            if (WorkOrder.DestinationWorkArea.WorkAreaSupervisorRoleId.HasValue)
            {
                await LoadSupervisorsAsync(WorkOrder.DestinationWorkArea.WorkAreaSupervisorRoleId.Value);
            }
        }

        private void ExitView()
        {
            // Return to the area view where the order originated from.
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(WorkUnitViewModel), Id = _originAreaId });
        }

        private void OnFilterByClientExecute(object isChecked)
        {
            if (((bool?)isChecked).HasValue)
            {
                if (((bool?)isChecked).Value)
                {
                    WorkUnitCollection.GroupDescriptions.Add(_clientName);
                }
                else
                {
                    WorkUnitCollection.GroupDescriptions.Remove(_clientName);
                }
            }
        }

        private void OnFilterByColorExecute(object isChecked)
        {
            if (((bool?)isChecked).HasValue)
            {
                if (((bool?)isChecked).Value)
                {
                    WorkUnitCollection.GroupDescriptions.Add(_colorName);
                }
                else
                {
                    WorkUnitCollection.GroupDescriptions.Remove(_colorName);
                }
            }
        }

        private void OnFilterByMaterialExecute(object isChecked)
        {
            if (((bool?)isChecked).HasValue)
            {
                if (((bool?)isChecked).Value)
                {
                    WorkUnitCollection.GroupDescriptions.Add(_materialName);
                }
                else
                {
                    WorkUnitCollection.GroupDescriptions.Remove(_materialName);
                }
            }
        }

        private void OnFilterByProductExecute(object isChecked)
        {
            if (((bool?)isChecked).HasValue)
            {
                if (((bool?)isChecked).Value)
                {
                    WorkUnitCollection.GroupDescriptions.Add(_productName);
                }
                else
                {
                    WorkUnitCollection.GroupDescriptions.Remove(_productName);
                }
            }
        }

        private string CreateWorkOrderReport()
        {
            // Create a new report class with the Work Order data.
            var workOrderReport = new WorkOrderReport
            {
                Id = WorkOrder.Id,
                CreationDateTime = WorkOrder.StartTime.ToString(),
                OriginWorkArea = WorkOrder.OriginWorkArea.Name,
                DestinationWorkArea = WorkOrder.DestinationWorkArea.Name,
                Responsible = WorkOrder.ResponsibleEmployee.FirstName + " " + WorkOrder.ResponsibleEmployee.LastName,
                Supervisor = WorkOrder.SupervisorEmployee.FirstName + " " + WorkOrder.SupervisorEmployee.LastName,
            };

            // Create the reports for each Work Unit in the Work Order.
            foreach (var workOrderUnit in WorkOrder.WorkOrderUnits)
            {
                var workUnit = workOrderUnit.WorkUnit;

                // If there is already a work unit in the report, check to group the similar ones
                // else just add the Work Unit.
                if (workOrderReport.WorkUnits.Count > 0)
                {
                    bool found = false;
                    foreach (var workUnitReport in workOrderReport.WorkUnits)
                    {
                        // If there is a work unit in the report that has the same properties, just add to the quantity.
                        if (workUnitReport.Product == workUnit.Product.Name
                            && workUnitReport.Material == workUnit.Material.Name
                            && workUnitReport.Color == workUnit.Color.Name)
                        {
                            workUnitReport.Quantity++;
                            found = true;
                            break;
                        }
                    }

                    // If there wasn't any work unit with the same properties, add the work unit to the report.
                    if (!found)
                    {
                        workOrderReport.WorkUnits.Add(new WorkUnitReport
                        {
                            Quantity = 1,
                            Product = workUnit.Product.Name,
                            Material = workUnit.Material.Name,
                            Color = workUnit.Color.Name,
                        });
                    }
                }
                else
                {
                    workOrderReport.WorkUnits.Add(new WorkUnitReport
                    {
                        Quantity = 1,
                        Product = workUnit.Product.Name,
                        Material = workUnit.Material.Name,
                        Color = workUnit.Color.Name,
                    });
                }
            }

            var rs = new ReportingService("http://127.0.0.1:5488","admin","mirno");
            workOrderReport.Id = WorkOrder.Id;
            var jsonString = JsonConvert.SerializeObject(workOrderReport);
            var report = rs.RenderByNameAsync("workorder-main", jsonString).Result;

            string filename = $"C:\\WorkOrders\\WorkOrder{WorkOrder.Id}.pdf";
            FileStream stream = new FileStream(filename, FileMode.Create);

            report.Content.CopyTo(stream);
            stream.Close();

            return filename;
        }

        private void SendToPrinter(string fileName)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = fileName;

            Process.Start(info);
        }
    }
}
