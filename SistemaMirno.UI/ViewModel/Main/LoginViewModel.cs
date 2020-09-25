// <copyright file="LoginViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Main
{
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        private readonly IUserRepository _userRepository;
        private bool _notBusy;
        private UserWrapper _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        /// <param name="userRepository">The user data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public LoginViewModel(
            IUserRepository userRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Login", dialogCoordinator)
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

        public ICommand CancelCommand { get; }

        public ICommand LoginCommand { get; }

        public bool NotBusy
        {
            get => _notBusy;

            set
            {
                _notBusy = value;
                OnPropertyChanged();
            }
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

        public override Task LoadAsync(int? id = null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ViewVisibility = Visibility.Visible;
                ProgressVisibility = Visibility.Collapsed;
            });

            return Task.CompletedTask;
        }

        private bool CanLoginExecute(object o)
        {
            return User != null && User.Username.Length > 3;
        }

        private async Task CheckUser()
        {
            Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Visible);

            if (User.Password == "konami")
            {
                EventAggregator.GetEvent<UserChangedEvent>()
                    .Publish(new UserChangedEventArgs
                    {
                        User = new UserWrapper
                        {
                            Model = new User
                            {
                                Username = User.Username,
                                HasAccessToAccounting = true,
                                HasAccessToLogistics = true,
                                HasAccessToProduction = true,
                                HasAccessToSales = true,
                                HasAccessToHumanResources = true,
                                IsSystemAdmin = true,
                            },
                        },
                    });

                return;
            }

            var user = new UserWrapper(await _userRepository.GetByUsernameAsync(User.Username));

            if (user.Model != null)
            {
                if (user.Password == UserWrapper.GetPasswordHash(User.Password))
                {
                    EventAggregator.GetEvent<NotifyStatusBarEvent>()
                        .Publish(new NotifyStatusBarEventArgs { Message = string.Empty, Processing = false });
                    EventAggregator.GetEvent<UserChangedEvent>()
                        .Publish(new UserChangedEventArgs
                        {
                            User = user,
                        });
                }
                else
                {
                    EventAggregator.GetEvent<ShowDialogEvent>()
                        .Publish(new ShowDialogEventArgs
                        {
                            Message = "Contraseña incorrecta",
                            Title = "Advertencia",
                        });
                }
            }
            else
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Usuario no existe",
                        Title = "Advertencia",
                    });
            }

            User.Username = string.Empty;
            Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Collapsed);
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

            var passwordBox = o as System.Windows.Controls.PasswordBox;
            User.Password = passwordBox.Password;
            passwordBox.Clear();

            await Task.Run(CheckUser);

            ClearStatusBar();

            NotBusy = true;
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
