// <copyright file="UserDetailViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    /// <summary>
    /// A class representing the view model for the user details view.
    /// </summary>
    public class UserDetailViewModel : DetailViewModelBase, IUserDetailViewModel
    {
        private IUserRepository _userRepository;
        private UserWrapper _user;
        private EmployeeWrapper _selectedEmployee;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetailViewModel"/> class.
        /// </summary>
        /// <param name="userRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public UserDetailViewModel(
            IUserRepository userRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Usuario", dialogCoordinator)
        {
            _userRepository = userRepository;

            Employees = new ObservableCollection<EmployeeWrapper>();
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
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

        public EmployeeWrapper SelectedEmployee
        {
            get => _selectedEmployee;

            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EmployeeWrapper> Employees { get; }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                User = new UserWrapper(user);
                User.PropertyChanged += User_PropertyChanged;
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            });

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();
            User.Password = User.GetPasswordHash(User.Password);

            var roles = await _userRepository.GetAllRolesFromEmployeeAsync(User.EmployeeId);

            foreach (var role in roles)
            {
                User.Model.HasAccessToAccounting = role.HasAccessToAccounting || User.Model.HasAccessToAccounting;
                User.Model.HasAccessToHumanResources = role.HasAccessToHumanResources || User.Model.HasAccessToHumanResources;
                User.Model.HasAccessToLogistics = role.HasAccessToLogistics || User.Model.HasAccessToLogistics;
                User.Model.HasAccessToProduction = role.HasAccessToProduction || User.Model.HasAccessToProduction;
                User.Model.HasAccessToSales = role.HasAccessToSales || User.Model.HasAccessToSales;
                User.Model.IsSystemAdmin = role.IsSystemAdmin || User.Model.IsSystemAdmin;
            }

            User.Model.Employee = SelectedEmployee.Model;

            if (IsNew)
            {
                await _userRepository.AddAsync(User.Model);
            }
            else
            {
                await _userRepository.SaveAsync(User.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(UserViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(User);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            await _userRepository.DeleteAsync(User.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(UserViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(UserViewModel),
                });
        }

        private void User_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _userRepository.HasChanges();
            }

            if (e.PropertyName == nameof(User.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id)
        {
            await LoadEmployees();

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            IsNew = true;

            Application.Current.Dispatcher.Invoke(() =>
            {
                User = new UserWrapper();
                User.PropertyChanged += User_PropertyChanged;
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();

                User.Username = string.Empty;
                User.Password = string.Empty;
                User.PasswordVerification = string.Empty;

                ProgressVisibility = Visibility.Collapsed;
            });
        }

        private async Task LoadEmployees()
        {
            var employees = await _userRepository.GetAllEmployeesAsync();

            foreach (var employee in employees)
            {
                Application.Current.Dispatcher.Invoke(() => Employees.Add(new EmployeeWrapper(employee)));
            }
        }
    }
}
