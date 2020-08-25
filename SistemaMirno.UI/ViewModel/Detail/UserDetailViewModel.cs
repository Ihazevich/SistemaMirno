// <copyright file="UserDetailViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

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

            // TODO: Set user permissions

            if (IsNew)
            {
                await _userRepository.AddAsync(User.Model);
            }
            else
            {
                await _userRepository.SaveAsync(User.Model);
            }
            HasChanges = false;
            EventAggregator.GetEvent<CloseDetailViewEvent<UserDetailViewModel>>()
                .Publish();
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
            EventAggregator.GetEvent<CloseDetailViewEvent<UserDetailViewModel>>()
                .Publish();
        }

        protected override void OnCancelExecute()
        {
            EventAggregator.GetEvent<CloseDetailViewEvent<UserDetailViewModel>>()
                .Publish();
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

        public override Task LoadAsync()
        {
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
            return Task.CompletedTask;
        }
    }
}
