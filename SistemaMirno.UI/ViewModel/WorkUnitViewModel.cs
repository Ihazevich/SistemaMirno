using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Event;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    /// <summary>
    /// View model for Work Units.
    /// </summary>
    public class WorkUnitViewModel : ViewModelBase, IWorkUnitViewModel
    {
        private IWorkUnitDataService _workUnitDataService;
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

        public ObservableCollection<WorkUnit> WorkUnits { get; set; }

        public WorkUnitViewModel(IWorkUnitDataService workUnitDataService,
            IEventAggregator eventAggregator)
        {
            WorkUnits = new ObservableCollection<WorkUnit>();
            _workUnitDataService = workUnitDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowWorkUnitsViewEvent>()
                .Subscribe(OnProductionAreaSelected);
        }

        private async void OnProductionAreaSelected(int productionAreaId)
        {
            await LoadAsync(productionAreaId);
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(this);
        }

        public async Task LoadAsync(int productionAreaId)
        {
            WorkUnits.Clear();
            AreaName = await _workUnitDataService.GetProductionAreaName(productionAreaId);
            var workUnits = await _workUnitDataService.GetWorkUnitsByAreaIdAsync(productionAreaId);

            foreach (var workUnit in workUnits)
            {
                WorkUnits.Add(workUnit);
            }
        }
    }
}