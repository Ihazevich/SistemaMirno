using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.ViewModel.General
{
    /// <summary>
    /// View model for Work Units.
    /// </summary>
    public class WorkUnitViewModel : ViewModelBase, IWorkUnitViewModel
    {
        private IWorkUnitRepository _workUnitRepository;
        private IEventAggregator _eventAggregator;
        private string _areaName;
        private int _areaId;

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

        public ICommand OpenWorkOrderViewCommand { get; }

        public ObservableCollection<WorkUnit> WorkUnits { get; set; }

        public WorkUnitViewModel(IWorkUnitRepository workUnitRepository,
            IEventAggregator eventAggregator)
        {
            _workUnitRepository = workUnitRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowViewEvent<WorkUnitViewModel>>()
                .Subscribe(OnWorkAreaSelected);

            WorkUnits = new ObservableCollection<WorkUnit>();
            OpenWorkOrderViewCommand = new DelegateCommand(OnOpenWorkOrderViewExecute);
        }

        private void OnOpenWorkOrderViewExecute()
        {
            _eventAggregator.GetEvent<ShowViewEvent<WorkOrderViewModel>>()
                .Publish(_areaId);
        }

        private async void OnWorkAreaSelected(int productionAreaId)
        {
            await LoadAsync(productionAreaId);
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(this);
        }

        public async Task LoadAsync(int productionAreaId)
        {
            WorkUnits.Clear();
            AreaName = await _workUnitRepository.GetWorkAreaNameAsync(productionAreaId);
            _areaId = productionAreaId;
            var workUnits = await _workUnitRepository.GetByAreaIdAsync(productionAreaId);

            foreach (var workUnit in workUnits)
            {
                WorkUnits.Add(workUnit);
            }
        }
    }
}