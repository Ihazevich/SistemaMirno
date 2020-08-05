using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
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
        private WorkUnitWrapper _selectedAreaWorkUnit;
        private WorkUnitWrapper _selectedOrderWorkUnit;
        private IEventAggregator _eventAggregator;
        private IWorkUnitRepository _workUnitRepository;

        public WorkUnitViewModel(IWorkUnitRepository workUnitRepository,
                    IEventAggregator eventAggregator)
        {
            _workUnitRepository = workUnitRepository;
            _eventAggregator = eventAggregator;

            AreaWorkUnits = new ObservableCollection<WorkUnitWrapper>();
            OrderWorkUnits = new ObservableCollection<WorkUnitWrapper>();
            OpenWorkOrderViewCommand = new DelegateCommand(OnOpenWorkOrderViewExecute);
            NewWorkOrderCommand = new DelegateCommand(OnNewWorkOrderExecute);
            AddWorkUnitCommand = new DelegateCommand(OnAddWorkUnitExecute);
            RemoveWorkUnitCommand = new DelegateCommand(OnRemoveWorkUnitExecute);
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

        public ICommand NewWorkOrderCommand { get; }

        public ICommand MoveToWorkAreaCommand { get; }

        public ICommand OpenWorkOrderViewCommand { get; }

        public ICommand AddWorkUnitCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

        public ObservableCollection<WorkUnitWrapper> AreaWorkUnits { get; set; }

        public ObservableCollection<WorkUnitWrapper> OrderWorkUnits { get; set; }

        public WorkUnitWrapper SelectedAreaWorkUnit
        {
            get => _selectedAreaWorkUnit;

            set
            {
                _selectedAreaWorkUnit = value;
                OnPropertyChanged();
            }
        }

        public WorkUnitWrapper SelectedOrderWorkUnit
        {
            get => _selectedOrderWorkUnit;

            set
            {
                _selectedOrderWorkUnit = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int? workAreaId)
        {
            if (workAreaId.HasValue)
            {
                AreaWorkUnits.Clear();
                AreaName = await _workUnitRepository.GetWorkAreaNameAsync(workAreaId.Value);
                _areaId = workAreaId.Value;
                var workUnits = await _workUnitRepository.GetByAreaIdAsync(workAreaId.Value);

                foreach (var workUnit in workUnits)
                {
                    AreaWorkUnits.Add(new WorkUnitWrapper(workUnit));
                }
            }
        }

        private void OnOpenWorkOrderViewExecute()
        {
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(WorkOrderViewModel), Id = _areaId });
        }

        private void OnNewWorkOrderExecute()
        {
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(WorkOrderDetailViewModel), Id = _areaId });
        }

        private void OnAddWorkUnitExecute()
        {
            OrderWorkUnits.Add(SelectedAreaWorkUnit);
            AreaWorkUnits.Remove(SelectedAreaWorkUnit);
        }

        private void OnRemoveWorkUnitExecute()
        {
            AreaWorkUnits.Add(SelectedOrderWorkUnit);
            OrderWorkUnits.Remove(SelectedOrderWorkUnit);
        }
    }
}