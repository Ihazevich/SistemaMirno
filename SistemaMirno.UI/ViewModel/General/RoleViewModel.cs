// <copyright file="RoleViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
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
    public class RoleViewModel : ViewModelBase
    {
        private readonly IRoleRepository _roleRepository;
        private RoleWrapper _selectedRole;

        public RoleViewModel(
            IRoleRepository branchRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Roles", dialogCoordinator)
        {
            _roleRepository = branchRepository;

            Roles = new ObservableCollection<RoleWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public ObservableCollection<RoleWrapper> Roles { get; }

        public RoleWrapper SelectedRole
        {
            get
            {
                return _selectedRole;
            }

            set
            {
                OnPropertyChanged();
                _selectedRole = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            Roles.Clear();

            var roles = await _roleRepository.GetAllAsync();

            foreach (var role in roles)
            {
                Application.Current.Dispatcher.Invoke(() => Roles.Add(new RoleWrapper(role)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RoleDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedRole != null;
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedRole.Id,
                    ViewModel = nameof(RoleDetailViewModel),
                });
        }
    }
}
