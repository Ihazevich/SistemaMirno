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
        private WorkAreaConnectionWrapper _selectedWorkAreaConnection;

        private string _workAreaWorkUnitProductFilter;
        private string _workAreaWorkUnitMaterialFilter;
        private string _workAreaWorkUnitColorFilter;
        private string _workAreaWorkUnitClientFilter;

        private PropertyGroupDescription _productName = new PropertyGroupDescription("Model.Product.Name");

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

            NewWorkOrderCommand = new DelegateCommand(OnNewWorkOrderExecute, OnNewWorkOrderCanExecute);
        }

        private bool OnNewWorkOrderCanExecute()
        {
            return WorkArea != null && WorkArea.Model.IncomingConnections.Count > 0;
        }

        private void OnNewWorkOrderExecute()
        {
            EventAggregator.GetEvent<NewWorkOrderEvent>()
                .Publish(new NewWorkOrderEventArgs()
                {
                    DestinationWorkAreaId = WorkArea.Id,
                    OriginWorkAreaId = WorkArea.Id,
                });
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
                ((DelegateCommand)NewWorkOrderCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkAreaConnectionWrapper SelectedWorkAreaConnection
        {
            get => _selectedWorkAreaConnection;

            set
            {
                _selectedWorkAreaConnection = value;
                OnPropertyChanged();
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

        public ObservableCollection<WorkAreaConnectionWrapper> WorkAreaConnections { get; set; }

        public ICollectionView WorkAreaCollectionView { get; }

        public ICollectionView WorkOrderCollectionView { get; }

        public ICommand NewWorkOrderCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                WorkAreaWorkUnits.Clear();
                _workUnitRepository = _workAreaRepositoryCreator();

                var workArea = await _workUnitRepository.GetWorkAreaById(id.Value);

                Application.Current.Dispatcher.Invoke(() => WorkArea = new WorkAreaWrapper(workArea));

                foreach (var workUnit in workArea.WorkUnits)
                {
                    Application.Current.Dispatcher.Invoke(() => WorkAreaWorkUnits.Add(new WorkUnitWrapper(workUnit)));
                }

                foreach (var connection in workArea.OutgoingConnections)
                {
                    Application.Current.Dispatcher.Invoke(() => WorkAreaConnections.Add(new WorkAreaConnectionWrapper(connection)));
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
    }
}
