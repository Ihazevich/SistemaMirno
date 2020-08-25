using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class RoleViewModel : ViewModelBase, IRoleViewModel
    {
        private IRoleRepository _roleRepository;
        private Func<IRoleRepository> _roleRepositoryCreator;
        private RoleWrapper _selectedRole;
        private IRoleDetailViewModel _roleDetailViewModel;
        private Func<IRoleDetailViewModel> _roleDetailViewModelCreator;

        public RoleViewModel(
            Func<IRoleDetailViewModel> roleDetailViewModelCreator,
            Func<IRoleRepository> branchRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Roles", dialogCoordinator)
        {
            _roleDetailViewModelCreator = roleDetailViewModelCreator;
            _roleRepositoryCreator = branchRepositoryCreator;

            Roles = new ObservableCollection<RoleWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);

            EventAggregator.GetEvent<CloseDetailViewEvent<RoleDetailViewModel>>()
                .Subscribe(CloseDetailView, ThreadOption.UIThread);
        }

        protected override async void CloseDetailView()
        {
            base.CloseDetailView();
            RoleDetailViewModel = null;

            await LoadAsync();
        }

        public IRoleDetailViewModel RoleDetailViewModel
        {
            get
            {
                return _roleDetailViewModel;
            }

            private set
            {
                _roleDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RoleWrapper> Roles { get; set; }

        public RoleWrapper SelectedRole
        {
            get
            {
                return _selectedRole;
            }

            set
            {
                OnPropertyChanged();
                _selectedRole = value;
                if (_selectedRole != null)
                {
                    UpdateDetailViewModel(_selectedRole.Id);
                }
            }
        }

        public ICommand CreateNewCommand { get; }

        public override async Task LoadAsync()
        {
            Roles.Clear();
            _roleRepository = _roleRepositoryCreator();

            var roles = await _roleRepository.GetAllAsync();

            foreach (var role in roles)
            {
                Application.Current.Dispatcher.Invoke(() => Roles.Add(new RoleWrapper(role)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private async Task UpdateDetailViewModel(int id)
        {
            if (RoleDetailViewModel != null && RoleDetailViewModel.HasChanges)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Guarde o descarte los cambios antes de salir",
                        Title = "Advertencia",
                    });
                return;
            }

            RoleDetailViewModel = _roleDetailViewModelCreator();
            await RoleDetailViewModel.LoadDetailAsync(id).ConfigureAwait(false);
        }

        private void OnCreateNewExecute()
        {
            if (RoleDetailViewModel != null && RoleDetailViewModel.HasChanges)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Guarde o descarte los cambios antes de salir",
                        Title = "Advertencia",
                    });
                return;
            }

            RoleDetailViewModel = _roleDetailViewModelCreator();
            RoleDetailViewModel.IsNew = true;
            RoleDetailViewModel.LoadAsync();
        }
    }
}
