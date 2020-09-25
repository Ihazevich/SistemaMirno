// <copyright file="UserViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
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
    /// <summary>
    /// A class representing the view model of the users view.
    /// </summary>
    public class UserViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;
        private UserWrapper _selectedUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="userDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public UserViewModel(
            IUserRepository userRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Usuarios", dialogCoordinator)
        {
            _userRepository = userRepository;

            Users = new ObservableCollection<UserWrapper>();

            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public UserWrapper SelectedUser
        {
            get => _selectedUser;

            set
            {
                OnPropertyChanged();
                _selectedUser = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<UserWrapper> Users { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Users.Clear();

            var users = await _userRepository.GetAllAsync();

            foreach (var user in users)
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(new UserWrapper(user)));
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
                    ViewModel = nameof(UserDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedUser != null;
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedUser.Id,
                    ViewModel = nameof(UserDetailViewModel),
                });
        }
    }
}
