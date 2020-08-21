using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class EmployeeDetailViewModel : DetailViewModelBase, IEmployeeDetailViewModel
    {
        private EmployeeWrapper _employee;
        private IEmployeeRepository _employeeRepository;
        private IEmployeeRoleRepository _employeeRoleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeDetailViewModel"/> class.
        /// </summary>
        /// <param name="productRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public EmployeeDetailViewModel(
            IEmployeeRepository employeeRepository,
            IEmployeeRoleRepository employeeRoleRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator, "Detalles de Empleado")
        {
            _employeeRepository = employeeRepository;
            _employeeRoleRepository = employeeRoleRepository;

            _eventAggregator.GetEvent<AfterDataModelSavedEvent<EmployeeRole>>()
                .Subscribe(AfterEmployeeRoleSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<EmployeeRole>>()
                .Subscribe(AfterEmployeeRoleDeleted);

            EmployeeRoles = new ObservableCollection<EmployeeRoleWrapper>();
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public EmployeeWrapper Employee
        {
            get
            {
                return _employee;
            }

            set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EmployeeRoleWrapper> EmployeeRoles { get; }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? employeeId)
        {
            var employee = employeeId.HasValue
                ? await _employeeRepository.GetByIdAsync(employeeId.Value)
                : CreateNewEmployee();

            Employee = new EmployeeWrapper(employee);
            Employee.PropertyChanged += Employee_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (employee.Id == 0)
            {
                // This triggers the validation.
                Employee.FirstName = string.Empty;
                Employee.LastName = string.Empty;
            }

            await LoadEmployeeRolesAsync();
        }

        protected override async void OnDeleteExecute()
        {
            _employeeRepository.Remove(Employee.Model);
            await _employeeRepository.SaveAsync();
            RaiseDataModelDeletedEvent(Employee.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Employee != null && !Employee.HasErrors && HasChanges;
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _employeeRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(Employee.Model);
        }

        private void AfterEmployeeRoleDeleted(AfterDataModelDeletedEventArgs<EmployeeRole> args)
        {
            var item = EmployeeRoles.SingleOrDefault(r => r.Id == args.Model.Id);

            if (item != null)
            {
                EmployeeRoles.Remove(item);
            }
        }

        private void AfterEmployeeRoleSaved(AfterDataModelSavedEventArgs<EmployeeRole> args)
        {
            var item = EmployeeRoles.SingleOrDefault(r => r.Id == args.Model.Id);

            if (item == null)
            {
                EmployeeRoles.Add(new EmployeeRoleWrapper(args.Model));
            }
            else
            {
                item.Name = args.Model.Name;
            }
        }

        private Employee CreateNewEmployee()
        {
            var employee = new Employee();
            _employeeRepository.Add(employee);
            return employee;
        }

        private void Employee_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
            if (!HasChanges)
            {
                HasChanges = _employeeRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Employee.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadEmployeeRolesAsync()
        {
            var roles = await _employeeRoleRepository.GetAllAsync();
            EmployeeRoles.Clear();
            foreach (var role in roles)
            {
                EmployeeRoles.Add(new EmployeeRoleWrapper(role));
            }
        }
    }
}
