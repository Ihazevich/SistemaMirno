// <copyright file="WorkAreaDetailViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class WorkAreaDetailViewModel : DetailViewModelBase
    {
        private readonly IWorkAreaRepository _workAreaRepository;
        private Branch _selectedBranch;
        private WorkArea _selectedWorkArea;
        private WorkAreaConnection _selectedWorkAreaConnection;
        private WorkAreaWrapper _workArea;

        public WorkAreaDetailViewModel(
            IWorkAreaRepository workAreaRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Area de Trabajo", dialogCoordinator)
        {
            _workAreaRepository = workAreaRepository;

            Roles = new ObservableCollection<Role>();
            Branches = new ObservableCollection<Branch>();
            WorkAreas = new ObservableCollection<WorkArea>();
            WorkAreaConnections = new ObservableCollection<WorkAreaConnection>();

            RolesCollectionView = CollectionViewSource.GetDefaultView(Roles);
            BranchesCollectionView = CollectionViewSource.GetDefaultView(Branches);
            WorkAreasCollectionView = CollectionViewSource.GetDefaultView(WorkAreas);

            AddConnectionCommand = new DelegateCommand(OnAddConnectionExecute, OnAddConnectionCanExecute);
            RemoveConnectionCommand = new DelegateCommand(OnRemoveConnectionExecute, OnRemoveConnectionCanExecute);
        }

        public ICommand AddConnectionCommand { get; }

        public ObservableCollection<Branch> Branches { get; }

        public ICollectionView BranchesCollectionView { get; }

        public ICommand RemoveConnectionCommand { get; }

        public ObservableCollection<Role> Roles { get; }

        public ICollectionView RolesCollectionView { get; }

        public Branch SelectedBranch
        {
            get => _selectedBranch;

            set
            {
                _selectedBranch = value;
                OnPropertyChanged();
                FilterRoles(_selectedBranch.Id);
            }
        }

        public WorkArea SelectedWorkArea
        {
            get => _selectedWorkArea;

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
                ((DelegateCommand)AddConnectionCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkAreaConnection SelectedWorkAreaConnection
        {
            get => _selectedWorkAreaConnection;

            set
            {
                _selectedWorkAreaConnection = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveConnectionCommand).RaiseCanExecuteChanged();
            }
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
                if (WorkArea != null)
                {
                    FilterWorkAreas();
                }
            }
        }

        public ObservableCollection<WorkAreaConnection> WorkAreaConnections { get; }

        public ObservableCollection<WorkArea> WorkAreas { get; }

        public ICollectionView WorkAreasCollectionView { get; }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadBranches();
            await LoadRoles();
            await LoadWorkAreas();

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

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(WorkArea);
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

            if (WorkArea.IsPassthrough)
            {
                try
                {
                    WorkArea.PassthroughWorkAreaId =
                        WorkAreas.Single(w => w.BranchId == WorkArea.BranchId && w.IsLast).Id;
                }
                catch (InvalidOperationException ex)
                {
                    EventAggregator.GetEvent<ShowDialogEvent>()
                        .Publish(new ShowDialogEventArgs
                        {
                            Title = "Error",
                            Message = $"¿Existe mas de un area final?\n[{ex.Message}]",
                        });
                }
                catch (Exception ex)
                {
                    EventAggregator.GetEvent<ShowDialogEvent>()
                        .Publish(new ShowDialogEventArgs
                        {
                            Title = "Error",
                            Message = $"Contacte al administrador de sistema, oh wait it's you hehe fix this >:C \n {ex.Message}",
                        });
                }
            }

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

        private void FilterRoles(int branchId)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                RolesCollectionView.Filter = item =>
                {
                    return item is Role vitem &&
                           vitem.BranchId == branchId;
                };
            });
        }

        private void FilterWorkAreas()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                WorkAreasCollectionView.Filter = item =>
                {
                    return item is WorkArea vitem &&
                           vitem.BranchId == WorkArea.BranchId && !string.Equals(vitem.Name, WorkArea.Name, StringComparison.CurrentCultureIgnoreCase);
                };
            });
        }

        private async Task LoadBranches()
        {
            var branches = await _workAreaRepository.GetAllBranchesAsync();

            foreach (var branch in branches)
            {
                Application.Current.Dispatcher.Invoke(() => Branches.Add(branch));
            }
        }

        private async Task LoadRoles()
        {
            var roles = await _workAreaRepository.GetAllRolesAsync();

            foreach (var role in roles)
            {
                Application.Current.Dispatcher.Invoke(() => Roles.Add(role));
            }
        }

        private async Task LoadWorkAreaConnections(int id)
        {
            var connections = await _workAreaRepository.GetAllWorkAreaConnectionsFromWorkAreaAsync(id);
            foreach (var connection in connections)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreaConnections.Add(connection));
            }
        }

        private async Task LoadWorkAreas()
        {
            var workAreas = await _workAreaRepository.GetAllWorkAreasAsync();

            foreach (var workArea in workAreas)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreas.Add(workArea));
            }
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

        private bool OnAddConnectionCanExecute()
        {
            return SelectedWorkArea != null;
        }

        private void OnAddConnectionExecute()
        {
            var connection = new WorkAreaConnection
            {
                DestinationWorkAreaId = SelectedWorkArea.Id,
                DestinationWorkArea = SelectedWorkArea,
            };

            WorkArea.Model.OutgoingConnections.Add(connection);
            WorkAreaConnections.Add(connection);
            HasChanges = true;
        }

        private bool OnRemoveConnectionCanExecute()
        {
            return SelectedWorkAreaConnection != null;
        }

        private void OnRemoveConnectionExecute()
        {
            WorkArea.Model.OutgoingConnections.Remove(SelectedWorkAreaConnection);
            WorkAreaConnections.Remove(SelectedWorkAreaConnection);
            HasChanges = true;
        }
    }
}
