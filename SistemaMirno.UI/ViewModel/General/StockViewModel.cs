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
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class StockViewModel : ViewModelBase, IStockViewModel
    {
        private IWorkUnitRepository _workUnitRepository;
        private WorkUnitWrapper _selectedWorkAreaWorkUnit;
        private BranchWrapper _selectedBranch;
        private WorkAreaWrapper _workArea;

        private string _workAreaWorkUnitProductFilter;
        private string _workAreaWorkUnitMaterialFilter;
        private string _workAreaWorkUnitColorFilter;
        private string _workAreaWorkUnitClientFilter;

        private bool _showAllBranches;

        private readonly PropertyGroupDescription _productName = new PropertyGroupDescription("Model.Product.Name");

        public StockViewModel(
            IWorkUnitRepository workUnitRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Unidades de Trabajo en Area", dialogCoordinator)
        {
            _workUnitRepository = workUnitRepository;

            WorkAreaWorkUnits = new ObservableCollection<WorkUnitWrapper>();
            Branches = new ObservableCollection<BranchWrapper>();

            WorkAreaCollectionView = CollectionViewSource.GetDefaultView(WorkAreaWorkUnits);
            WorkAreaCollectionView.GroupDescriptions.Add(_productName);

            NewSaleCommand = new DelegateCommand(OnNewSaleExecute);
            MoveToBranchCommand = new DelegateCommand(OnMoveToBranchExecute);
            ShowMovementHistoryCommand = new DelegateCommand(OnShowMovementHistoryExecute);

            WorkAreaWorkUnitProductFilter = string.Empty;
            WorkAreaWorkUnitMaterialFilter = string.Empty;
            WorkAreaWorkUnitColorFilter = string.Empty;
            WorkAreaWorkUnitClientFilter = string.Empty;
        }

        private void OnMoveToBranchExecute()
        {
            throw new NotImplementedException();
        }

        private void OnShowMovementHistoryExecute()
        {
            throw new NotImplementedException();
        }

        private void OnNewSaleExecute()
        {
            /*
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    ViewModel = nameof(SaleDetailViewModel);
                });
            */
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

        public bool ShowAllBranches
        {
            get => _showAllBranches;

            set
            {
                _showAllBranches = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BranchSelectionEnabled));
                if (value)
                {
                    FilterWorkAreaCollection();
                }
            }
        }

        public bool BranchSelectionEnabled => !ShowAllBranches;

        public WorkAreaWrapper WorkArea
        {
            get => _workArea;

            set
            {
                _workArea = value;
                OnPropertyChanged();
            }
        }

        public WorkUnitWrapper SelectedWorkAreaWorkUnit
        {
            get => _selectedWorkAreaWorkUnit;

            set
            {
                _selectedWorkAreaWorkUnit = value;
                OnPropertyChanged();
            }
        }

        public BranchWrapper SelectedBranch
        {
            get => _selectedBranch;

            set
            {
                _selectedBranch = value;
                OnPropertyChanged();
                FilterWorkAreaCollection();
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
                     (vitem.Model.Requisition.Client != null &&
                      vitem.Model.Requisition.Client.FullName.ToLowerInvariant()
                          .Contains(WorkAreaWorkUnitClientFilter
                              .ToLowerInvariant()))) &&

                    // If not showing all branches, filter by branch
                    (ShowAllBranches ||
                     (vitem.Model.CurrentWorkArea != null &&
                      SelectedBranch != null &&
                      vitem.Model.CurrentWorkArea.Branch.Id == SelectedBranch.Id));
                ProgressVisibility = Visibility.Hidden;
            });

        }

        public ObservableCollection<WorkUnitWrapper> WorkAreaWorkUnits { get; }
        
        public ObservableCollection<BranchWrapper> Branches { get; }

        public ICollectionView WorkAreaCollectionView { get; }
        
        public ICommand NewSaleCommand { get; }

        public ICommand MoveToBranchCommand { get; }

        public ICommand ShowMovementHistoryCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {

            if (id.HasValue)
            {
                WorkAreaWorkUnits.Clear();

                await LoadLastWorkArea(id.Value);
                await LoadWorkUnits();
                await LoadBranches();

                ShowAllBranches = true;

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

        private async Task LoadLastWorkArea(int id)
        {
            var workArea = await _workUnitRepository.GetLastWorkAreaFromBranchIdAsync(id);

            Application.Current.Dispatcher.Invoke(() => WorkArea = new WorkAreaWrapper(workArea));
        }

        private async Task LoadWorkUnits()
        {
            var workUnits = await _workUnitRepository.GetAllWorkUnitsInAllLastWorkAreasAsync();

            foreach (var workUnit in workUnits)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreaWorkUnits.Add(new WorkUnitWrapper(workUnit)));
            }
        }

        private async Task LoadBranches()
        {
            var branches = await _workUnitRepository.GetAllBranchesAsync();

            foreach (var branch in branches)
            {
                Application.Current.Dispatcher.Invoke(() => Branches.Add(new BranchWrapper(branch)));
            }
        }
    }
}
