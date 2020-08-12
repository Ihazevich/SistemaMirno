// <copyright file="UserDetailViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
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
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public UserWrapper User
        {
            get
            {
                return _user;
            }

            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? userId)
        {
            var user = userId.HasValue
                ? await _userRepository.GetByIdAsync(userId.Value)
                : CreateNewUser();

            User = new UserWrapper(user);
            User.PropertyChanged += User_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (user.Id == 0)
            {
                // This triggers the validation.
                User.Name = string.Empty;
            }
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _userRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(User.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return User != null && !User.HasErrors && HasChanges;
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            _userRepository.Remove(User.Model);
            await _userRepository.SaveAsync();
            RaiseDataModelDeletedEvent(User.Model);
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

        private User CreateNewUser()
        {
            var user = new User();
            _userRepository.Add(user);
            return user;
        }
    }
}
