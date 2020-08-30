using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class WorkOrderViewModel : ViewModelBase, IWorkOrderView
    {
        private IWorkOrderRepository _workOrderRepository;
        private WorkOrderWrapper _selectedWorkOrder;
        private WorkAreaWrapper _selectedWorkArea;

        private bool _showAllWorkAreas;
        private bool _showSingleDay;

        private DateTime _fromDate;
        private DateTime _toDate;

        public WorkOrderViewModel(
            IWorkOrderRepository workOrderRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Ordenes de trabajo", dialogCoordinator)
        {
            _workOrderRepository = workOrderRepository;

            WorkAreas = new ObservableCollection<WorkAreaWrapper>();
            WorkOrders = new ObservableCollection<WorkOrderWrapper>();
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
            ReloadWorkOrdersCommand = new DelegateCommand(OnReloadWorkOrdersExecute);
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedWorkOrder.Id,
                    ViewModel = nameof(WorkOrderDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedWorkOrder != null;
        }

        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; }

        public ObservableCollection<WorkOrderWrapper> WorkOrders { get; }

        public WorkOrderWrapper SelectedWorkOrder
        {
            get
            {
                return _selectedWorkOrder;
            }

            set
            {
                OnPropertyChanged();
                _selectedWorkOrder = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkAreaWrapper SelectedWorkArea
        {
            get => _selectedWorkArea;

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
            }
        }

        public bool ShowSingleDay
        {
            get => _showSingleDay;

            set
            {
                _showSingleDay = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ToDateEnabled));
            }
        }

        public bool ShowAllWorkAreas
        {
            get => _showAllWorkAreas;

            set
            {
                _showAllWorkAreas = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WorkAreaSelectionEnabled));
            }
        }

        public bool ToDateEnabled => !ShowSingleDay;

        public bool WorkAreaSelectionEnabled => !ShowAllWorkAreas;

        public DateTime FromDate
        {
            get => _fromDate;

            set
            {
                _fromDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime ToDate
        {
            get => _toDate;

            set
            {
                _toDate = value;
                OnPropertyChanged();
            }
        }

        private async Task ReloadWorkOrders()
        {
            WorkOrders.Clear();

            var workAreasIds = new List<int>();

            if (ShowAllWorkAreas)
            {
                workAreasIds = WorkAreas.Select(w => w.Id).ToList();
            }
            else
            {
                workAreasIds.Add(SelectedWorkArea.Id);
            }

            if (ShowSingleDay)
            {
                Application.Current.Dispatcher.Invoke(() => ToDate = FromDate);
            }

            var workOrders =
                await _workOrderRepository.GetAllWorkOrdersFromWorkAreasBetweenDatesAsync(
                    workAreasIds,
                    FromDate,
                    ToDate);

            foreach (var workOrder in workOrders)
            {
                Application.Current.Dispatcher.Invoke(() => WorkOrders.Add(new WorkOrderWrapper(workOrder)));
            }
        }

        private async void OnReloadWorkOrdersExecute()
        {
            await ReloadWorkOrders().ConfigureAwait(false);
        }

        public ICommand OpenDetailCommand { get; }

        public ICommand ReloadWorkOrdersCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            WorkOrders.Clear();

            var workAreas = await _workOrderRepository.GetAllWorkAreasAsync(SessionInfo.Branch.Id);

            foreach (var workArea in workAreas)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreas.Add(new WorkAreaWrapper(workArea)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                FromDate = DateTime.Today;
                ToDate = DateTime.Today;
                ShowSingleDay = true;
                ShowAllWorkAreas = true;
            });

            if (id.HasValue)
            {
                ShowAllWorkAreas = false;
                SelectedWorkArea = WorkAreas.Single(w => w.Id == id.Value);
            }

            await ReloadWorkOrders();

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }
    }
}
