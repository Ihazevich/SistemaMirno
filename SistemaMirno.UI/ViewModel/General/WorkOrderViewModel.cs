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
        private IWorkOrderRepository _workOrderRepository;
        private IEventAggregator _eventAggregator;
        private string _areaName;

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

        public WorkOrderViewModel(
            IWorkOrderRepository workOrderRepository,
            IEventAggregator eventAggregator)
        {
            _workOrderRepository = workOrderRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowViewEvent<WorkOrderViewModel>>()
                .Subscribe(OnAreaWorkOrderSelected);

            WorkOrders = new ObservableCollection<WorkOrder>();
        }

        private async void OnAreaWorkOrderSelected(int workAreaId)
        {
            await LoadAsync(workAreaId);
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(this);
        }

        public async Task LoadAsync(int productionAreaId)
        {
            WorkOrders.Clear();
            AreaName = await _workOrderRepository.GetWorkAreaNameAsync(productionAreaId);
            var workUnits = await _workOrderRepository.GetByAreaIdAsync(productionAreaId);

            foreach (var workUnit in workUnits)
            {
                WorkOrders.Add(workUnit);
            }
        }
    }
}
