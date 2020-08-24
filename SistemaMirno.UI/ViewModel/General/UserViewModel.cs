// <copyright file="UserViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;
using MessageDialogResult = SistemaMirno.UI.View.Services.MessageDialogResult;

namespace SistemaMirno.UI.ViewModel.General
{
    /// <summary>
    /// A class representing the view model of the users view.
    /// </summary>
    public class UserViewModel : ViewModelBase, IUserViewModel
    {
        private IUserRepository _userRepository;
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
            IUserRepository userRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Usuarios", dialogCoordinator)
        {
            _userDetailViewModelCreator = userDetailViewModelCreator;
            _userRepository = userRepository;

            Users = new ObservableCollection<UserWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
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
            
            var users = await Task.Run(() => _userRepository.GetAllAsync());
            foreach (var user in users)
            {
                Users.Add(new UserWrapper(user));
            }

            ProgressVisibility = Visibility.Collapsed;
            ViewVisibility = Visibility.Visible;
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
            UserDetailViewModel.LoadDetailAsync(id);
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
            UserDetailViewModel.CreateNew();
        }
    }
}
