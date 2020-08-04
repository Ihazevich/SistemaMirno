using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    /// <summary>
    /// View model for Work Units.
    /// </summary>
    public class WorkUnitViewModel : ViewModelBase, IWorkUnitViewModel
    {
        private int _areaId;
        private string _areaName;
        private IEventAggregator _eventAggregator;
        private IWorkUnitRepository _workUnitRepository;

        public WorkUnitViewModel(IWorkUnitRepository workUnitRepository,
                    IEventAggregator eventAggregator)
        {
            _workUnitRepository = workUnitRepository;
            _eventAggregator = eventAggregator;

            WorkUnits = new ObservableCollection<WorkUnitWrapper>();
            OpenWorkOrderViewCommand = new DelegateCommand(OnOpenWorkOrderViewExecute);
            OpenWorkOrderDetailViewCommand = new DelegateCommand(OnOpenWorkOrderDetailViewExecute);
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

        public ICommand OpenWorkOrderDetailViewCommand { get; }

        public ICommand OpenWorkOrderViewCommand { get; }

        public ObservableCollection<WorkUnitWrapper> WorkUnits { get; set; }

        public override async Task LoadAsync(int? workAreaId)
        {
            if (workAreaId.HasValue)
            {
                WorkUnits.Clear();
                AreaName = await _workUnitRepository.GetWorkAreaNameAsync(workAreaId.Value);
                _areaId = workAreaId.Value;
                var workUnits = await _workUnitRepository.GetByAreaIdAsync(workAreaId.Value);

                foreach (var workUnit in workUnits)
                {
                    WorkUnits.Add(new WorkUnitWrapper(workUnit));
                }
            }
        }

        private void OnOpenWorkOrderViewExecute()
        {
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(WorkOrderViewModel), Id = _areaId });
        }

        private void OnOpenWorkOrderDetailViewExecute()
        {
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(WorkOrderDetailViewModel), Id = _areaId });
        }
    }
}