// <copyright file="WorkUnitViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class WorkUnitViewModel : ViewModelBase
    {
        private readonly PropertyGroupDescription _productName = new PropertyGroupDescription("Model.Description");
        private readonly IWorkUnitRepository _workUnitRepository;
        private WorkAreaConnection _selectedWorkAreaConnection;
        private WorkUnitWrapper _selectedWorkAreaWorkUnit;
        private WorkUnitWrapper _selectedWorkOrderWorkUnit;
        private WorkAreaWrapper _workArea;
        private string _workAreaWorkUnitClientFilter;
        private string _workAreaWorkUnitColorFilter;
        private string _workAreaWorkUnitMaterialFilter;
        private string _workAreaWorkUnitProductFilter;

        public WorkUnitViewModel(
            IWorkUnitRepository workUnitRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Unidades de Trabajo en Area", dialogCoordinator)
        {
            _workUnitRepository = workUnitRepository;

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
            DeleteWorkUnitCommand = new DelegateCommand(OnDeleteWorkUnitExecute, OnDeleteWorkUnitCanExecute);
            ShowWorkUnitDetailsCommand = new DelegateCommand(OnShowWorkUnitDetailsExecute, OnShowWorkUnitDetailsCanExecute);

            WorkAreaWorkUnitProductFilter = string.Empty;
            WorkAreaWorkUnitMaterialFilter = string.Empty;
            WorkAreaWorkUnitColorFilter = string.Empty;
            WorkAreaWorkUnitClientFilter = string.Empty;
        }

        public ICommand AddWorkUnitCommand { get; }

        public Visibility DeleteWorkUnitButtonVisibility { get; set; }

        public ICommand DeleteWorkUnitCommand { get; }

        public string MoveOrderButtonText => SelectedWorkAreaConnection != null
            ? $"MOVER A {SelectedWorkAreaConnection.DestinationWorkArea.Name.ToUpperInvariant()}"
            : "MOVER";

        public ICommand NewWorkOrderCommand { get; }

        public ICommand OpenWorkAreaMovementViewCommand { get; }

        public ICommand OpenWorkOrderViewCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

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
                ((DelegateCommand)DeleteWorkUnitCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)ShowWorkUnitDetailsCommand).RaiseCanExecuteChanged();
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

        public ICommand ShowWorkUnitDetailsCommand { get; }

        public long TotalProductionValue { get; set; }

        public long TotalRetailPrice { get; set; }

        public Visibility TotalsVisibility =>
            SessionInfo.User.Model.IsSystemAdmin ? Visibility.Visible : Visibility.Collapsed;

        public long TotalWholesalerPrice { get; set; }

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

        public ICollectionView WorkAreaCollectionView { get; }

        public ObservableCollection<WorkAreaConnectionWrapper> WorkAreaConnections { get; }

        public string WorkAreaWorkUnitClientFilter
        {
            get => _workAreaWorkUnitClientFilter;

            set
            {
                _workAreaWorkUnitClientFilter = value;
                OnPropertyChanged();
                FilterWorkAreaCollection();
            }
        }

        public string WorkAreaWorkUnitColorFilter
        {
            get => _workAreaWorkUnitColorFilter;

            set
            {
                _workAreaWorkUnitColorFilter = value;
                OnPropertyChanged();
                FilterWorkAreaCollection();
            }
        }

        public string WorkAreaWorkUnitMaterialFilter
        {
            get => _workAreaWorkUnitMaterialFilter;

            set
            {
                _workAreaWorkUnitMaterialFilter = value;
                OnPropertyChanged();
                FilterWorkAreaCollection();
            }
        }

        public string WorkAreaWorkUnitProductFilter
        {
            get => _workAreaWorkUnitProductFilter;

            set
            {
                _workAreaWorkUnitProductFilter = value;
                OnPropertyChanged();
                FilterWorkAreaCollection();
            }
        }

        public ObservableCollection<WorkUnitWrapper> WorkAreaWorkUnits { get; }

        public ICollectionView WorkOrderCollectionView { get; }

        public ObservableCollection<WorkUnitWrapper> WorkOrderWorkUnits { get; }

        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                WorkAreaWorkUnits.Clear();

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
                    DeleteWorkUnitButtonVisibility = SessionInfo.User.Model.IsSystemAdmin ? Visibility.Visible : Visibility.Collapsed;
                    ProgressVisibility = Visibility.Collapsed;
                    ViewVisibility = Visibility.Visible;
                    OnPropertyChanged(nameof(TotalsVisibility));
                    OnPropertyChanged(nameof(TotalProductionValue));
                    OnPropertyChanged(nameof(TotalWholesalerPrice));
                    OnPropertyChanged(nameof(TotalRetailPrice));
                });
            }
            else
            {
                EventAggregator.GetEvent<ChangeViewEvent>()
                    .Publish(new ChangeViewEventArgs());
            }
        }

        private void FilterWorkAreaCollection()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Visible;
                WorkAreaCollectionView.Filter = item =>
                    item is WorkUnitWrapper vitem &&

                    // Filter by description
                    vitem.Model.Description.ToLowerInvariant()
                        .Contains(WorkAreaWorkUnitProductFilter
                            .ToLowerInvariant()) &&

                    // Filter by material
                    vitem.Model.Material.Name.ToLowerInvariant()
                        .Contains(WorkAreaWorkUnitMaterialFilter
                            .ToLowerInvariant()) &&

                    // Filter by color
                    vitem.Model.Color.Name.ToLowerInvariant()
                        .Contains(WorkAreaWorkUnitColorFilter.ToLowerInvariant()) &&

                    // If client name not empty check if item has client, then filter by client
                    (WorkAreaWorkUnitClientFilter == string.Empty ||
                     (vitem.Model.Requisition?.Client != null &&
                      vitem.Model.Requisition.Client.FullName.ToLowerInvariant()
                         .Contains(WorkAreaWorkUnitClientFilter
                             .ToLowerInvariant())));
                ProgressVisibility = Visibility.Hidden;
            });
        }

        private async Task LoadConnections(int id)
        {
            var connections = await _workUnitRepository.GetWorkAreaOutgoingConnectionsAsync(id);

            foreach (var connection in connections)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreaConnections.Add(new WorkAreaConnectionWrapper(connection)));
            }
        }

        private async Task LoadWorkArea(int id)
        {
            var workArea = await _workUnitRepository.GetWorkAreaByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() => WorkArea = new WorkAreaWrapper(workArea));
        }

        private async Task LoadWorkUnits(int id)
        {
            var workUnits = await _workUnitRepository.GetAllWorkUnitsCurrentlyInWorkAreaAsync(id);

            TotalProductionValue = 0;
            TotalWholesalerPrice = 0;
            TotalRetailPrice = 0;

            foreach (var workUnit in workUnits)
            {
                TotalProductionValue += workUnit.Product.ProductionValue;
                TotalWholesalerPrice += workUnit.Product.WholesalerPrice;
                TotalRetailPrice += workUnit.Product.RetailPrice;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WorkAreaWorkUnits.Add(new WorkUnitWrapper(workUnit));
                });
            }
        }

        private bool OnAddWorkUnitCommandCanExecute()
        {
            return SelectedWorkAreaWorkUnit != null;
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

            ((DelegateCommand)AddWorkUnitCommand).RaiseCanExecuteChanged();
            ((DelegateCommand<object>)NewWorkOrderCommand).RaiseCanExecuteChanged();
        }

        private bool OnDeleteWorkUnitCanExecute()
        {
            return SelectedWorkAreaWorkUnit != null;
        }

        private async void OnDeleteWorkUnitExecute()
        {
            await _workUnitRepository.DeleteAsync(SelectedWorkAreaWorkUnit.Model);
            Application.Current.Dispatcher.Invoke(() => WorkAreaWorkUnits.Remove(SelectedWorkAreaWorkUnit));
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

        private void OnOpenWorkAreaMovementViewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = WorkArea.Id,
                    ViewModel = nameof(WorkAreaMovementViewModel),
                });
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

        private bool OnRemoveWorkUnitCanExecute()
        {
            return SelectedWorkOrderWorkUnit != null;
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

            ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
            ((DelegateCommand<object>)NewWorkOrderCommand).RaiseCanExecuteChanged();
        }

        private bool OnShowWorkUnitDetailsCanExecute()
        {
            return SelectedWorkAreaWorkUnit != null;
        }

        private void OnShowWorkUnitDetailsExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    ViewModel = nameof(WorkUnitDetailViewModel),
                    Id = SelectedWorkAreaWorkUnit.Id,
                });
        }
    }
}
