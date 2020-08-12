// <copyright file="LoginViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Main
{
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        private IUserRepository _userRepository;
        private IEventAggregator _eventAggregator;
        private UserWrapper _user;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="userRepository">The user data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public LoginViewModel(
            IUserRepository userRepository,
            IEventAggregator eventAggregator)
        {
            _userRepository = userRepository;
            _eventAggregator = eventAggregator;

            LoginCommand = new DelegateCommand(OnLoginExecute, CanLoginExecute);
            CancelCommand = new DelegateCommand(OnCancelExecute);

            User = new UserWrapper(new User());
            User.PropertyChanged += User_PropertyChanged;

            // Trigger validation.
            User.Name = string.Empty;
            User.Password = string.Empty;
        }

        private void OnCancelExecute()
        {
            _eventAggregator.GetEvent<ExitApplicationEvent>()
                .Publish();
        }

        private async void OnLoginExecute()
        {
            await CheckUser();
        }

        private bool CanLoginExecute()
        {
            return User != null && !User.HasErrors;
        }

        private async Task CheckUser()
        {
            var user = new UserWrapper(await _userRepository.GetByNameAsync(User.Name));

            if (user.Model != null)
            {
                if (user.Password == User.Password)
                {
                    _eventAggregator.GetEvent<UserChangedEvent>()
                        .Publish(new UserChangedEventArgs { Username = User.Name, AccessLevel = User.AccessLevel });
                }
            }

            User.Name = string.Empty;
            User.Password = string.Empty;
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public UserWrapper User
        {
            get => _user;

            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public ICommand CancelCommand { get; }

        public override Task LoadAsync(int? id)
        {
            return null;
        }

        private void User_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(User.HasErrors))
            {
                ((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
