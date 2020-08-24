// <copyright file="UserViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    /// <summary>
    /// A class representing the view model of the users view.
    /// </summary>
    public class UserViewModel : ViewModelBase, IUserViewModel
    {
        private IUserRepository _userRepository;
        private IMessageDialogService _messageDialogService;
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
            IMessageDialogService messageDialogService)
            : base(eventAggregator, "Usuarios")
        {
            _userDetailViewModelCreator = userDetailViewModelCreator;
            _userRepository = userRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<User>>()
                .Subscribe(AfterUserSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<User>>()
                .Subscribe(AfterUserDeleted);

            Users = new ObservableCollection<UserWrapper>();
            CreateNewUserCommand = new DelegateCommand(OnCreateNewUserExecute);
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
        public ICommand CreateNewUserCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync()
        {
            Users.Clear();
            var users = await _userRepository.GetAllAsync();
            foreach (var user in users)
            {
                Users.Add(new UserWrapper(user));
            }
        }

        private async void UpdateDetailViewModel(int id)
        {
            if (UserDetailViewModel != null && UserDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios que no han sido guardados, estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            UserDetailViewModel = _userDetailViewModelCreator();
            UserDetailViewModel.LoadDetailAsync(id);
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterUserSaved(AfterDataModelSavedEventArgs<User> args)
        {
            var item = Users.SingleOrDefault(c => c.Id == args.Model.Id);

            if (item == null)
            {
                Users.Add(new UserWrapper(args.Model));
                UserDetailViewModel = null;
            }
            else
            {
                item.Username = args.Model.Username;
            }
        }

        private void AfterUserDeleted(AfterDataModelDeletedEventArgs<User> args)
        {
            var item = Users.SingleOrDefault(m => m.Id == args.Model.Id);

            if (item != null)
            {
                Users.Remove(item);
            }

            UserDetailViewModel = null;
        }

        private void OnCreateNewUserExecute()
        {
            if (UserDetailViewModel != null && UserDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios que no han sido guardados, estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            UserDetailViewModel = _userDetailViewModelCreator();
            UserDetailViewModel.CreateNew();
        }
    }
}
