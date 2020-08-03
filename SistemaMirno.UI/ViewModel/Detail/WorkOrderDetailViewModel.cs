using System;
using System.Threading.Tasks;
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
        private IEventAggregator _eventAggregator;
        private WorkOrderWrapper _workOrder;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderDetailViewModel"/> class.
        /// </summary>
        /// <param name="workOrderRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public WorkOrderDetailViewModel(
            IWorkOrderRepository workOrderRepository,
            IEventAggregator eventAggregator)
        {
            _workOrderRepository = workOrderRepository;
            _eventAggregator = eventAggregator;
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

        /// <summary>
        /// Gets or sets a value indicating whether the database context has changes.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return _hasChanges;
            }

            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        /// <inheritdoc/>
        public async Task LoadAsync(int WorkOrderId)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(WorkOrderId);

            WorkOrder = new WorkOrderWrapper(workOrder);
            WorkOrder.PropertyChanged += ProductionArea_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            // TODO: Here goes the validation triggers
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _workOrderRepository.SaveAsync();
            //HasChanges = _productionAreaRepository.HasChanges();
            HasChanges = false;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<WorkOrder>>()
                .Publish(new AfterDataModelSavedEventArgs<WorkOrder> { Model = WorkOrder.Model });
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
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<WorkOrder>>()
                .Publish(new AfterDataModelDeletedEventArgs<WorkOrder> { Model = WorkOrder.Model });
        }

        private void ProductionArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
    }
}
