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
    public class EmployeeRoleViewModel : ViewModelBase, IEmployeeRoleViewModel
    {
        private IEmployeeRoleRepository _employeeRoleRepository;
        private IMessageDialogService _messageDialogService;
        private EmployeeRoleWrapper _selectedEmployeeRole;
        private IEmployeeRoleDetailViewModel _employeeRoleDetailViewModel;
        private Func<IEmployeeRoleDetailViewModel> _employeeRoleDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRoleViewModel"/> class.
        /// </summary>
        /// <param name="employeeRoleDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public EmployeeRoleViewModel(
            Func<IEmployeeRoleDetailViewModel> employeeRoleDetailViewModelCreator,
            IEmployeeRoleRepository employeeRoleRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
            : base (eventAggregator, "Roles")
        {
            _employeeRoleDetailViewModelCreator = employeeRoleDetailViewModelCreator;
            _employeeRoleRepository = employeeRoleRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<EmployeeRole>>()
                .Subscribe(AfterEmployeeRoleSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<EmployeeRole>>()
                .Subscribe(AfterEmployeeRoleDeleted);

            EmployeeRoles = new ObservableCollection<EmployeeRoleWrapper>();
            CreateNewEmployeeRoleCommand = new DelegateCommand(OnCreateNewEmployeeRoleExecute);
        }

        /// <summary>
        /// Gets the Color detail view model.
        /// </summary>
        public IEmployeeRoleDetailViewModel EmployeeRoleDetailViewModel
        {
            get
            {
                return _employeeRoleDetailViewModel;
            }

            private set
            {
                _employeeRoleDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Colors.
        /// </summary>
        public ObservableCollection<EmployeeRoleWrapper> EmployeeRoles { get; set; }

        /// <summary>
        /// Gets or sets the selected Color.
        /// </summary>
        public EmployeeRoleWrapper SelectedEmployeeRole
        {
            get
            {
                return _selectedEmployeeRole;
            }

            set
            {
                OnPropertyChanged();
                _selectedEmployeeRole = value;
                if (_selectedEmployeeRole != null)
                {
                    UpdateDetailViewModel(_selectedEmployeeRole.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewColor command.
        /// </summary>
        public ICommand CreateNewEmployeeRoleCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync(int? id)
        {
            EmployeeRoles.Clear();
            var roles = await _employeeRoleRepository.GetAllAsync();
            foreach (var role in roles)
            {
                EmployeeRoles.Add(new EmployeeRoleWrapper(role));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (EmployeeRoleDetailViewModel != null && EmployeeRoleDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            EmployeeRoleDetailViewModel = _employeeRoleDetailViewModelCreator();
            EmployeeRoleDetailViewModel.LoadAsync(id);
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterEmployeeRoleSaved(AfterDataModelSavedEventArgs<EmployeeRole> args)
        {
            var item = EmployeeRoles.SingleOrDefault(r => r.Id == args.Model.Id);

            if (item == null)
            {
                EmployeeRoles.Add(new EmployeeRoleWrapper(args.Model));
                EmployeeRoleDetailViewModel = null;
            }
            else
            {
                item.Name = args.Model.Name;
            }
        }

        private void AfterEmployeeRoleDeleted(AfterDataModelDeletedEventArgs<EmployeeRole> args)
        {
            var item = EmployeeRoles.SingleOrDefault(r => r.Id == args.Model.Id);

            if (item != null)
            {
                EmployeeRoles.Remove(item);
            }

            EmployeeRoleDetailViewModel = null;
        }

        private void OnCreateNewEmployeeRoleExecute()
        {
            UpdateDetailViewModel(null);
        }
    }
}
