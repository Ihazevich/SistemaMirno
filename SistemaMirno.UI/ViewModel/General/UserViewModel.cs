// <copyright file="UserViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    /// <summary>
    /// A class representing the view model of the users view.
    /// </summary>
    public class UserViewModel : ViewModelBase, IUserViewModel
    {
        private IUserRepository _userRepository;
        private Func<IUserRepository> _userRepositoryCreator;
        private UserWrapper _selectedUser;
        private IUserDetailViewModel _userDetailViewModel;
        private Func<IUserDetailViewModel> _userDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="userDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public UserViewModel(
            Func<IUserDetailViewModel> userDetailViewModelCreator,
            Func<IUserRepository> userRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Usuarios", dialogCoordinator)
        {
            _userDetailViewModelCreator = userDetailViewModelCreator;
            _userRepositoryCreator = userRepositoryCreator;

            Users = new ObservableCollection<UserWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);

            EventAggregator.GetEvent<CloseDetailViewEvent<UserDetailViewModel>>()
                .Subscribe(CloseDetailView);
        }

        private void CloseDetailView()
        {
            base.CloseDetailView();
            UserDetailViewModel = null;
        }

        /// <summary>
        /// Gets the User detail view model.
        /// </summary>
        public IUserDetailViewModel UserDetailViewModel
        {
            get
            {
                return _userDetailViewModel;
            }

            private set
            {
                _userDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Users.
        /// </summary>
        public ObservableCollection<UserWrapper> Users { get; set; }

        /// <summary>
        /// Gets or sets the selected User.
        /// </summary>
        public UserWrapper SelectedUser
        {
            get
            {
                return _selectedUser;
            }

            set
            {
                OnPropertyChanged();
                _selectedUser = value;
                if (_selectedUser != null)
                {
                    UpdateDetailViewModel(_selectedUser.Id);
                }
            }
        }

        /// <summary>
        /// Gets the create new user command.
        /// </summary>
        public ICommand CreateNewCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync()
        {
            Users.Clear();
            _userRepository = _userRepositoryCreator();

            var users = await _userRepository.GetAllAsync();

            foreach (var user in users)
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(new UserWrapper()));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private async void UpdateDetailViewModel(int id)
        {
            if (UserDetailViewModel != null && UserDetailViewModel.HasChanges)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Guarde o descarte los cambios antes de salir",
                        Title = "Advertencia",
                    });
                return;
            }

            UserDetailViewModel = _userDetailViewModelCreator();
            await UserDetailViewModel.LoadDetailAsync(id);
        }
        
        private void OnCreateNewExecute()
        {
            if (UserDetailViewModel != null && UserDetailViewModel.HasChanges)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Guarde o descarte los cambios antes de salir",
                        Title = "Advertencia",
                    });
                return;
            }

            UserDetailViewModel = _userDetailViewModelCreator();
            UserDetailViewModel.IsNew = true;
            UserDetailViewModel.LoadAsync();
        }
    }
}
