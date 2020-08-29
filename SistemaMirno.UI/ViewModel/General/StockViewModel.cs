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
                    FilterWorkAreaCollection(string.Empty, 4);
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
                FilterWorkAreaCollection(_selectedBranch.Name, 4);
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

                // Branch
                case 4:
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
                                return vitem.Model.CurrentWorkArea != null && string.Equals(
                                    vitem.Model.CurrentWorkArea.Branch.Name,
                                    value,
                                    StringComparison.InvariantCultureIgnoreCase);
                            }

                        };
                        ProgressVisibility = Visibility.Hidden;
                    });
                    break;
            }
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
