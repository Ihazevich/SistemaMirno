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
    public class EmployeeViewModel : ViewModelBase, IEmployeeViewModel
    {
        private IEmployeeRepository _employeeRepository;
        private IMessageDialogService _messageDialogService;
        private IEventAggregator _eventAggregator;
        private EmployeeWrapper _selectedEmployee;
        private IEmployeeDetailViewModel _employeeDetailViewModel;
        private Func<IEmployeeDetailViewModel> _employeeDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public EmployeeViewModel(
            Func<IEmployeeDetailViewModel> employeeDetailViewModelCreator,
            IEmployeeRepository employeeRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _employeeDetailViewModelCreator = employeeDetailViewModelCreator;
            _employeeRepository = employeeRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<Employee>>()
                .Subscribe(AfterEmployeeSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<Employee>>()
                .Subscribe(AfterEmployeeDeleted);

            Employees = new ObservableCollection<EmployeeWrapper>();
            CreateNewEmployeeCommand = new DelegateCommand(OnCreateNewEmployeeExecute);
        }

        /// <summary>
        /// Gets the Employee detail view model.
        /// </summary>
        public IEmployeeDetailViewModel EmployeeDetailViewModel
        {
            get
            {
                return _employeeDetailViewModel;
            }

            private set
            {
                _employeeDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Employees.
        /// </summary>
        public ObservableCollection<EmployeeWrapper> Employees { get; set; }

        /// <summary>
        /// Gets or sets the selected Employee.
        /// </summary>
        public EmployeeWrapper SelectedEmployee
        {
            get
            {
                return _selectedEmployee;
            }

            set
            {
                OnPropertyChanged();
                _selectedEmployee = value;
                if (_selectedEmployee != null)
                {
                    UpdateDetailViewModel(_selectedEmployee.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewProduct command.
        /// </summary>
        public ICommand CreateNewEmployeeCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public async Task LoadAsync(int id)
        {
            Employees.Clear();
            var employees = await _employeeRepository.GetAllAsync();
            foreach (var employee in employees)
            {
                Employees.Add(new EmployeeWrapper(employee));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (EmployeeDetailViewModel != null && EmployeeDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            EmployeeDetailViewModel = _employeeDetailViewModelCreator();
            await EmployeeDetailViewModel.LoadAsync(id);
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterEmployeeSaved(AfterDataModelSavedEventArgs<Employee> args)
        {
            var item = Employees.SingleOrDefault(e => e.Id == args.Model.Id);

            if (item == null)
            {
                Employees.Add(new EmployeeWrapper(args.Model));
                EmployeeDetailViewModel = null;
            }
            else
            {
                item.FirstName = args.Model.FirstName;
                item.LastName = args.Model.LastName;
            }
        }

        private void AfterEmployeeDeleted(AfterDataModelDeletedEventArgs<Employee> args)
        {
            var item = Employees.SingleOrDefault(e => e.Id == args.Model.Id);

            if (item != null)
            {
                Employees.Remove(item);
            }

            EmployeeDetailViewModel = null;
        }

        private void OnCreateNewEmployeeExecute()
        {
            UpdateDetailViewModel(null);
        }
    }
}
