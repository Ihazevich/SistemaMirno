// <copyright file="StockViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class StockViewModel : ViewModelBase
    {
        private readonly IWorkUnitRepository _workUnitRepository;
        private BranchWrapper _selectedBranch;
        private WorkUnitWrapper _selectedWorkAreaWorkUnit;
        private bool _showAllBranches;
        private WorkAreaWrapper _workArea;
        private string _workAreaWorkUnitClientFilter;
        private string _workAreaWorkUnitColorFilter;
        private string _workAreaWorkUnitMaterialFilter;
        private string _workAreaWorkUnitProductFilter;

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
            WorkAreaCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Model.Product.Name"));

            NewSaleCommand = new DelegateCommand(OnNewSaleExecute);
            DeleteWorkUnitCommand = new DelegateCommand(OnDeleteWorkUnitExecute, OnDeleteWorkUnitCanExecute);

            WorkAreaWorkUnitProductFilter = string.Empty;
            WorkAreaWorkUnitMaterialFilter = string.Empty;
            WorkAreaWorkUnitColorFilter = string.Empty;
            WorkAreaWorkUnitClientFilter = string.Empty;
        }

        public ObservableCollection<BranchWrapper> Branches { get; }

        public bool BranchSelectionEnabled => !ShowAllBranches;

        public ICommand DeleteWorkUnitCommand { get; }

        public ICommand NewSaleCommand { get; }

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

        public WorkUnitWrapper SelectedWorkAreaWorkUnit
        {
            get => _selectedWorkAreaWorkUnit;

            set
            {
                _selectedWorkAreaWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)DeleteWorkUnitCommand).RaiseCanExecuteChanged();
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
            }
        }

        public ICollectionView WorkAreaCollectionView { get; }

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

                    // If client and requisition exist, filter by client
                    (WorkAreaWorkUnitClientFilter == string.Empty ||
                     (vitem.Model.Requisition?.Client != null &&
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

        private async Task LoadBranches()
        {
            var branches = await _workUnitRepository.GetAllBranchesAsync();

            foreach (var branch in branches)
            {
                Application.Current.Dispatcher.Invoke(() => Branches.Add(new BranchWrapper(branch)));
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

        private bool OnDeleteWorkUnitCanExecute()
        {
            return SelectedWorkAreaWorkUnit != null;
        }

        private async void OnDeleteWorkUnitExecute()
        {
            await _workUnitRepository.DeleteAsync(SelectedWorkAreaWorkUnit.Model);
            Application.Current.Dispatcher.Invoke(() => WorkAreaWorkUnits.Remove(SelectedWorkAreaWorkUnit));
        }

        private void OnNewSaleExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    ViewModel = nameof(SaleDetailViewModel),
                });
        }
    }
}
