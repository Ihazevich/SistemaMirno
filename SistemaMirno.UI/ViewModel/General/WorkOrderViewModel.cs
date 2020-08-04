using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.ViewModel.General
{
    public class WorkOrderViewModel : ViewModelBase, IWorkOrderViewModel
    {
        private string _areaName;
        private IEventAggregator _eventAggregator;
        private IWorkOrderRepository _workOrderRepository;

        public WorkOrderViewModel(
                    IWorkOrderRepository workOrderRepository,
                    IEventAggregator eventAggregator)
        {
            _workOrderRepository = workOrderRepository;
            _eventAggregator = eventAggregator;

            WorkOrders = new ObservableCollection<WorkOrder>();
        }

        /// <summary>
        /// Gets or sets the production area name for the view.
        /// </summary>
        public string AreaName
        {
            get
            {
                return _areaName;
            }

            set
            {
                _areaName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<WorkOrder> WorkOrders { get; set; }

        public override async Task LoadAsync(int? workAreaId)
        {
            if (workAreaId.HasValue)
            {
                WorkOrders.Clear();
                AreaName = await _workOrderRepository.GetWorkAreaNameAsync(workAreaId.Value);
                var workUnits = await _workOrderRepository.GetByAreaIdAsync(workAreaId.Value);

                foreach (var workUnit in workUnits)
                {
                    WorkOrders.Add(workUnit);
                }
            }
        }
    }
}
