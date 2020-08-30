using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class WorkUnitViewModel : ViewModelBase, IWorkUnitViewModel
    {
        private readonly Func<IWorkUnitRepository> _workAreaRepositoryCreator;
        private IWorkUnitRepository _workUnitRepository;
        private WorkUnitWrapper _selectedWorkAreaWorkUnit;
        private WorkUnitWrapper _selectedWorkOrderWorkUnit;
        private WorkAreaWrapper _workArea;
        private WorkAreaConnection _selectedWorkAreaConnection;

        private string _workAreaWorkUnitProductFilter;
        private string _workAreaWorkUnitMaterialFilter;
        private string _workAreaWorkUnitColorFilter;
        private string _workAreaWorkUnitClientFilter;
        
        private readonly PropertyGroupDescription _productName = new PropertyGroupDescription("Model.Product.Name");

        public WorkUnitViewModel(
            Func<IWorkUnitRepository> workUnitRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Unidades de Trabajo en Area", dialogCoordinator)
        {
            _workAreaRepositoryCreator = workUnitRepositoryCreator;

            WorkAreaWorkUnits = new ObservableCollection<WorkUnitWrapper>();
            WorkOrderWorkUnits = new ObservableCollection<WorkUnitWrapper>();
            WorkAreaConnections = new ObservableCollection<WorkAreaConnectionWrapper>();

            WorkAreaCollectionView = CollectionViewSource.GetDefaultView(WorkAreaWorkUnits);
            WorkOrderCollectionView = CollectionViewSource.GetDefaultView(WorkOrderWorkUnits);
            WorkAreaCollectionView.GroupDescriptions.Add(_productName);
            WorkOrderCollectionView.GroupDescriptions.Add(_productName);

            NewWorkOrderCommand = new DelegateCommand<object>(OnNewWorkOrderExecute, OnNewWorkOrderCanExecute);
            OpenWorkOrderViewCommand = new DelegateCommand(OnOpenWorkOrderViewExecute);
            AddWorkUnitCommand = new DelegateCommand(OnAddWorkUnitCommandExecute, OnAddWorkUnitCommandCanExecute);
            RemoveWorkUnitCommand = new DelegateCommand(OnRemoveWorkUnitExecute, OnRemoveWorkUnitCanExecute);
            OpenWorkAreaMovementViewCommand = new DelegateCommand(OnOpenWorkAreaMovementViewExecute);
        }

        private void OnOpenWorkAreaMovementViewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = WorkArea.Id,
                    ViewModel = nameof(WorkAreaMovementViewModel),
                });
        }

        public string MoveOrderButtonText => SelectedWorkAreaConnection != null
            ? $"Trasladar a {SelectedWorkAreaConnection.DestinationWorkArea.Name}"
            : "Trasladar";

        private bool OnRemoveWorkUnitCanExecute()
        {
            return SelectedWorkOrderWorkUnit != null;
        }

        private bool OnAddWorkUnitCommandCanExecute()
        {
            return SelectedWorkAreaWorkUnit != null;
        }

        private void OnRemoveWorkUnitExecute()
        {
            while (SelectedWorkOrderWorkUnit != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WorkAreaWorkUnits.Add(SelectedWorkOrderWorkUnit);
                    WorkOrderWorkUnits.Remove(SelectedWorkOrderWorkUnit);
                });
            }
        }

        private void OnAddWorkUnitCommandExecute()
        {
            while (SelectedWorkAreaWorkUnit != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WorkOrderWorkUnits.Add(SelectedWorkAreaWorkUnit);
                    WorkAreaWorkUnits.Remove(SelectedWorkAreaWorkUnit);
                });
            }
        }

        private void OnOpenWorkOrderViewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = WorkArea.Id,
                    ViewModel = nameof(WorkOrderViewModel),
                });
        }

        private bool OnNewWorkOrderCanExecute(object args)
        {
            if (WorkArea != null)
            {
                switch (args.ToString())
                {
                    case "New":
                        if (WorkArea.Model.IncomingConnections != null)
                        {
                            return WorkArea.Model.IncomingConnections.Count > 0;
                        }

                        return false;

                    case "Move":
                        if (WorkArea.Model.OutgoingConnections != null)
                        {
                            return WorkOrderWorkUnits.Count > 0 && SelectedWorkAreaConnection != null && WorkArea.Model.OutgoingConnections.Count > 0;
                        }

                        return false;

                    default:
                        return false;
                }
            }

            return false;
        }

        private void OnNewWorkOrderExecute(object args)
        {
            switch (args.ToString())
            {
                case "New":
                    EventAggregator.GetEvent<NewWorkOrderEvent>()
                        .Publish(new NewWorkOrderEventArgs()
                        {
                            DestinationWorkAreaId = WorkArea.Id,
                            OriginWorkAreaId = WorkArea.Id,
                        });
                    break;

                case "Move":
                    EventAggregator.GetEvent<NewWorkOrderEvent>()
                        .Publish(new NewWorkOrderEventArgs()
                        {
                            DestinationWorkAreaId = SelectedWorkAreaConnection.DestinationWorkAreaId,
                            OriginWorkAreaId = WorkArea.Id,
                            WorkUnits = WorkOrderWorkUnits,
                        });
                    break;
            }
        }

        public string WorkAreaWorkUnitProductFilter
        {
            get => _workAreaWorkUnitProductFilter;

            set
            {
                _workAreaWorkUnitProductFilter = value;
                OnPropertyChanged();
                FilterWorkAreaCollection(value, 0);
            }
        }

        public string WorkAreaWorkUnitMaterialFilter
        {
            get => _workAreaWorkUnitMaterialFilter;

            set
            {
                _workAreaWorkUnitMaterialFilter = value;
                OnPropertyChanged();
                FilterWorkAreaCollection(value, 1);
            }
        }

        public string WorkAreaWorkUnitColorFilter
        {
            get => _workAreaWorkUnitColorFilter;

            set
            {
                _workAreaWorkUnitColorFilter = value;
                OnPropertyChanged();
                FilterWorkAreaCollection(value, 2);
            }
        }

        public string WorkAreaWorkUnitClientFilter
        {
            get => _workAreaWorkUnitClientFilter;

            set
            {
                _workAreaWorkUnitClientFilter = value;
                OnPropertyChanged();
                FilterWorkAreaCollection(value, 3);
            }
        }

        public WorkAreaWrapper WorkArea
        {
            get => _workArea;

            set
            {
                _workArea = value;
                OnPropertyChanged();
                ((DelegateCommand<object>)NewWorkOrderCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkAreaConnection SelectedWorkAreaConnection
        {
            get => _selectedWorkAreaConnection;

            set
            {
                _selectedWorkAreaConnection = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MoveOrderButtonText));
                ((DelegateCommand<object>)NewWorkOrderCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkUnitWrapper SelectedWorkAreaWorkUnit
        {
            get => _selectedWorkAreaWorkUnit;

            set
            {
                _selectedWorkAreaWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)AddWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkUnitWrapper SelectedWorkOrderWorkUnit
        {
            get => _selectedWorkOrderWorkUnit;

            set
            {
                _selectedWorkOrderWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        private void FilterWorkAreaCollection(string value, int columnId)
        {
            switch (columnId)
            {
                // Product
                case 0:
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProgressVisibility = Visibility.Visible;
                        WorkAreaCollectionView.Filter = item =>
                        {
                            WorkUnitWrapper vitem = item as WorkUnitWrapper;
                            return vitem != null && vitem.Model.Product.Name.ToLowerInvariant().Contains(value.ToLowerInvariant());
                        };
                        ProgressVisibility = Visibility.Hidden;
                    });
                    break;

                // Material
                case 1:
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProgressVisibility = Visibility.Visible;
                        WorkAreaCollectionView.Filter = item =>
                        {
                            WorkUnitWrapper vitem = item as WorkUnitWrapper;
                            return vitem != null && vitem.Model.Material.Name.ToLowerInvariant().Contains(value.ToLowerInvariant());
                        };
                        ProgressVisibility = Visibility.Hidden;
                    });
                    break;

                // Color
                case 2:
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProgressVisibility = Visibility.Visible;
                        WorkAreaCollectionView.Filter = item =>
                        {
                            WorkUnitWrapper vitem = item as WorkUnitWrapper;
                            return vitem != null && vitem.Model.Color.Name.ToLowerInvariant().Contains(value.ToLowerInvariant());
                        };
                        ProgressVisibility = Visibility.Hidden;
                    });
                    break;

                // Client
                case 3:
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProgressVisibility = Visibility.Visible;
                        WorkAreaCollectionView.Filter = item =>
                        {
                            if (!(item is WorkUnitWrapper vitem))
                            {
                                return false;
                            }

                            if (value == string.Empty)
                            {
                                return true;
                            }
                            else
                            {
                                return vitem.Model.Requisition.Client != null && vitem.Model.Requisition.Client
                                    .FullName.ToLowerInvariant().Contains(value.ToLowerInvariant());
                            }

                        };
                        ProgressVisibility = Visibility.Hidden;
                    });
                    break;
            }
        }

        public ObservableCollection<WorkUnitWrapper> WorkAreaWorkUnits { get; }

        public ObservableCollection<WorkUnitWrapper> WorkOrderWorkUnits { get; }

        public ObservableCollection<WorkAreaConnectionWrapper> WorkAreaConnections { get; }

        public ICollectionView WorkAreaCollectionView { get; }

        public ICollectionView WorkOrderCollectionView { get; }

        public ICommand NewWorkOrderCommand { get; }

        public ICommand OpenWorkOrderViewCommand { get; }

        public ICommand AddWorkUnitCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

        public ICommand OpenWorkAreaMovementViewCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                WorkAreaWorkUnits.Clear();
                _workUnitRepository = _workAreaRepositoryCreator();

                try
                {
                    await LoadWorkArea(id.Value);
                    await LoadWorkUnits(id.Value);
                    await LoadConnections(id.Value);
                }
                catch (Exception ex)
                {
                    EventAggregator.GetEvent<ShowDialogEvent>()
                        .Publish(new ShowDialogEventArgs
                        {
                            Message = ex.Message,
                        });
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ProgressVisibility = Visibility.Collapsed;
                    ViewVisibility = Visibility.Visible;
                });
            }
            else
            {
                EventAggregator.GetEvent<ChangeViewEvent>()
                    .Publish(new ChangeViewEventArgs());
            }
        }

        private async Task LoadWorkArea(int id)
        {
            var workArea = await _workUnitRepository.GetWorkAreaById(id);

            Application.Current.Dispatcher.Invoke(() => WorkArea = new WorkAreaWrapper(workArea));
        }

        private async Task LoadWorkUnits(int id)
        {
            var workUnits = await _workUnitRepository.GetAllWorkUnitsCurrentlyInWorkArea(id);

            foreach (var workUnit in workUnits)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreaWorkUnits.Add(new WorkUnitWrapper(workUnit)));
            }
        }

        private async Task LoadConnections(int id)
        {
            var connections = await _workUnitRepository.GetWorkAreaOutgoingConnections(id);

            foreach (var connection in connections)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreaConnections.Add(new WorkAreaConnectionWrapper(connection)));
            }
        }
    }
}
