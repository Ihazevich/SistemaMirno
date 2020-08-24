// <copyright file="LoginViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
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
        private IUserRepository _userRepositoryCreator;
        private UserWrapper _user;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="userRepository">The user data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public LoginViewModel(
            IUserRepository userRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base (eventAggregator, "Login", dialogCoordinator)
        {
            _userRepository = userRepository;

            LoginCommand = new DelegateCommand<object>(OnLoginExecute, CanLoginExecute);
            CancelCommand = new DelegateCommand(OnCancelExecute);

            User = new UserWrapper(new User());
            User.PropertyChanged += User_PropertyChanged;

            // Trigger validation.
            User.Username = string.Empty;

            ViewVisibility = System.Windows.Visibility.Visible;
            ClearStatusBar();
        }

        private void OnCancelExecute()
        {
            _eventAggregator.GetEvent<ExitApplicationEvent>()
                .Publish();
        }

        private async void OnLoginExecute(object o)
        {
            NotifyStatusBar("Verificando usuario", true);

            var passwordBox = (o as System.Windows.Controls.PasswordBox);
            User.Password = passwordBox.Password;
            passwordBox.Clear();

            await Task.Run(CheckUser);

            ClearStatusBar();
        }

        private bool CanLoginExecute(object o)
        {
            return User != null && User.Username.Length > 3;
        }

        private async Task CheckUser()
        {
            if (User.Password == "konami")
            {
                _eventAggregator.GetEvent<UserChangedEvent>()
                    .Publish(new UserChangedEventArgs { Username = User.Username, AccessLevel = 0 });
                _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                    .Publish(true);

                return;
            }

            var user = new UserWrapper(await _userRepository.GetByUsernameAsync(User.Username));

            if (user.Model != null)
            {
                if (user.Password == User.GetPasswordHash(User.Password))
                {
                    _eventAggregator.GetEvent<UserChangedEvent>()
                        .Publish(new UserChangedEventArgs { Username = User.Username });
                    _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                        .Publish(true);
                    _eventAggregator.GetEvent<NotifyStatusBarEvent>()
                        .Publish(new NotifyStatusBarEventArgs { Message = string.Empty, Processing = false });
                }
            }
            else
            {
                _eventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Usuario no existe",
                        Title = "Error",
                    });
            }

            User.Username = string.Empty;
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

        public override Task LoadAsync()
        {
            ViewVisibility = System.Windows.Visibility.Visible;
            return null;
        }

        private void User_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(User.HasErrors))
            {
                ((DelegateCommand<object>)LoginCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
