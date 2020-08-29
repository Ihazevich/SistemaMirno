using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class WorkAreaDetailViewModel : DetailViewModelBase, IWorkAreaDetailViewModel
    {
        private IWorkAreaRepository _workAreaRepository;
        private WorkAreaWrapper _workArea;
        private WorkAreaWrapper _selectedWorkArea;
        private WorkAreaConnectionWrapper _selectedWorkAreaConnection;
        private BranchWrapper _selectedBranch;

        public WorkAreaDetailViewModel(
            IWorkAreaRepository workAreaRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Area de Trabajo", dialogCoordinator)
        {
            _workAreaRepository = workAreaRepository;

            Roles = new ObservableCollection<RoleWrapper>();
            Branches = new ObservableCollection<BranchWrapper>();
            WorkAreas = new ObservableCollection<WorkAreaWrapper>();
            WorkAreaConnections = new ObservableCollection<WorkAreaConnectionWrapper>();

            AddConnectionCommand = new DelegateCommand(OnAddConnectionExecute, OnAddConnectionCanExecute);
            RemoveConnectionCommand = new DelegateCommand(OnRemoveConnectionExecute, OnRemoveConnectionCanExecute);
        }

        private void OnAddConnectionExecute()
        {
            var connection = new WorkAreaConnection
            {
                DestinationWorkAreaId = SelectedWorkArea.Id,
                DestinationWorkArea = SelectedWorkArea.Model,
            };

            WorkArea.Model.OutgoingConnections.Add(connection);
            WorkAreaConnections.Add(new WorkAreaConnectionWrapper(connection));
            HasChanges = true;
        }

        private void OnRemoveConnectionExecute()
        {
            WorkArea.Model.OutgoingConnections.Remove(SelectedWorkAreaConnection.Model);
            WorkAreaConnections.Remove(SelectedWorkAreaConnection);
            HasChanges = true;
        }

        private bool OnAddConnectionCanExecute()
        {
            return SelectedWorkArea != null;
        }

        private bool OnRemoveConnectionCanExecute()
        {
            return SelectedWorkAreaConnection != null;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public WorkAreaWrapper WorkArea
        {
            get => _workArea;

            set
            {
                _workArea = value;
                OnPropertyChanged();
            }
        }

        public WorkAreaWrapper SelectedWorkArea
        {
            get => _selectedWorkArea;

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
                ((DelegateCommand)AddConnectionCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkAreaConnectionWrapper SelectedWorkAreaConnection
        {
            get => _selectedWorkAreaConnection;

            set
            {
                _selectedWorkAreaConnection = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveConnectionCommand).RaiseCanExecuteChanged();
            }
        }
        
        public BranchWrapper SelectedBranch
        {
            get => _selectedBranch;

            set
            {
                _selectedBranch = value;
                OnPropertyChanged();
                SelectedBranchChanged(_selectedBranch.Id);
            }
        }


        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; }

        public ObservableCollection<WorkAreaConnectionWrapper> WorkAreaConnections { get; }

        public ObservableCollection<BranchWrapper> Branches { get; }

        public ObservableCollection<RoleWrapper> Roles { get; }

        public ICommand AddConnectionCommand { get; }

        public ICommand RemoveConnectionCommand { get; }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _workAreaRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                WorkArea = new WorkAreaWrapper(model);
                WorkArea.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            await LoadWorkAreaConnections(id);

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            if (WorkArea.IsFirst && await _workAreaRepository.CheckIfFirstExistsAsync(WorkArea.BranchId))
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Area Inicial ya existe",
                        Title = "Advertencia",
                    });
                return;
            }

            if (WorkArea.IsLast && await _workAreaRepository.CheckIfLastExistsAsync(WorkArea.BranchId))
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Area Final ya existe",
                        Title = "Advertencia",
                    });
                return;
            }

            base.OnSaveExecute();
            if (IsNew)
            {

                await _workAreaRepository.AddAsync(WorkArea.Model);
            }
            else
            {
                await _workAreaRepository.SaveAsync(WorkArea.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ReloadNavigationViewEvent>()
                .Publish();
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(WorkAreaViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(WorkArea);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            base.OnDeleteExecute();

            await _workAreaRepository.DeleteAsync(WorkArea.Model);

            EventAggregator.GetEvent<ReloadNavigationViewEvent>()
                .Publish();
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(WorkAreaViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            base.OnCancelExecute();

            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(WorkAreaViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _workAreaRepository.HasChanges();
            }

            if (e.PropertyName == nameof(WorkArea.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadBranches();

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                WorkArea = new WorkAreaWrapper();
                WorkArea.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                WorkArea.Name = string.Empty;
                WorkArea.IsFirst = false;
                WorkArea.IsLast = false;
                WorkArea.Position = "0";
                WorkArea.ReportsInProcess = false;
                WorkArea.ResponsibleRoleId = 0;
                WorkArea.SupervisorRoleId = 0;
                WorkArea.BranchId = 0;

                ProgressVisibility = Visibility.Collapsed;
            });
        }

        private async Task LoadBranches()
        {
            var branches = await _workAreaRepository.GetAllBranchesAsync();

            foreach (var branch in branches)
            {
                Application.Current.Dispatcher.Invoke(() => Branches.Add(new BranchWrapper(branch)));
            }
        }

        private async Task LoadRoles(int id)
        {
            var roles = await _workAreaRepository.GetAllRolesFromBranchAsync(id);

            foreach (var role in roles)
            {
                Application.Current.Dispatcher.Invoke(() => Roles.Add(new RoleWrapper(role)));
            }
        }

        private async Task LoadWorkAreas(int id)
        {
            var workAreas = await _workAreaRepository.GetAllWorkAreasFromBranchAsync(id);

            foreach (var workArea in workAreas.Where(workArea => workArea.Name != WorkArea.Name || workArea.BranchId != WorkArea.BranchId))
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreas.Add(new WorkAreaWrapper(workArea)));
            }
        }

        private async Task LoadWorkAreaConnections(int id)
        {
            var connections = await _workAreaRepository.GetWorkAreaConnectionsFromWorkAreaBranchAsync(id);

            foreach (var connection in connections)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreaConnections.Add(new WorkAreaConnectionWrapper(connection)));
            }
        }

        private async Task SelectedBranchChanged(int id)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsEnabled = false;
                ProgressVisibility = Visibility.Visible;
            });

            await LoadWorkAreas(id);
            await LoadRoles(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsEnabled = true;
                ProgressVisibility = Visibility.Collapsed;
            });
        }
    }
}
