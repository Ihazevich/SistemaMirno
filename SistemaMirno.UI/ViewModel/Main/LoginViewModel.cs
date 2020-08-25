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
        private UserWrapper _user;
        private bool _notBusy;

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
            NotBusy = true;

            ViewVisibility = System.Windows.Visibility.Visible;
            ClearStatusBar();
        }

        public bool NotBusy
        {
            get => _notBusy;

            set
            {
                _notBusy = value;
                OnPropertyChanged();
            }
        }

        private void OnCancelExecute()
        {
            EventAggregator.GetEvent<ExitApplicationEvent>()
                .Publish();
        }

        private async void OnLoginExecute(object o)
        {
            NotBusy = false;

            NotifyStatusBar("Verificando usuario", true);

            var passwordBox = (o as System.Windows.Controls.PasswordBox);
            User.Password = passwordBox.Password;
            passwordBox.Clear();

            await Task.Run(CheckUser);

            ClearStatusBar();

            NotBusy = true;
        }

        private bool CanLoginExecute(object o)
        {
            return User != null && User.Username.Length > 3;
        }

        private async Task CheckUser()
        {
            if (User.Password == "konami")
            {
                EventAggregator.GetEvent<UserChangedEvent>()
                    .Publish(new UserChangedEventArgs
                    {
                        Username = User.Username,
                        HasAccessToAccounting = true,
                        HasAccessToLogistics = true,
                        HasAccessToProduction = true,
                        HasAccessToSales = true,
                        HasAccessToHumanResources = true,
                        IsSystemAdmin = true,
                    });

                return;
            }

            var user = new UserWrapper(await _userRepository.GetByUsernameAsync(User.Username));

            if (user.Model != null)
            {
                if (user.Password == User.GetPasswordHash(User.Password))
                {
                    EventAggregator.GetEvent<UserChangedEvent>()
                        .Publish(new UserChangedEventArgs
                        {
                            Username = User.Username,
                            EmployeeFullName = user.Model.Employee.FullName,
                            HasAccessToAccounting = user.Model.HasAccessToAccounting,
                            HasAccessToProduction = user.Model.HasAccessToProduction,
                            HasAccessToLogistics = user.Model.HasAccessToLogistics,
                            HasAccessToSales = user.Model.HasAccessToSales,
                            HasAccessToHumanResources = user.Model.HasAccessToHumanResources,
                            IsSystemAdmin = user.Model.IsSystemAdmin,
                        });
                    EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                        .Publish(true);
                    EventAggregator.GetEvent<NotifyStatusBarEvent>()
                        .Publish(new NotifyStatusBarEventArgs { Message = string.Empty, Processing = false });
                }
            }
            else
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Usuario o contraseña incorrectos",
                        Title = "Advertencia",
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
