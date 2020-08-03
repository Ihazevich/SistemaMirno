using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class EmployeeDetailViewModel : DetailViewModelBase, IEmployeeDetailViewModel
    {
        private IEmployeeRepository _employeeRepository;
        private IEmployeeRoleRepository _employeeRoleRepository;
        private IEventAggregator _eventAggregator;
        private EmployeeWrapper _employee;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeDetailViewModel"/> class.
        /// </summary>
        /// <param name="productRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public EmployeeDetailViewModel(
            IEmployeeRepository employeeRepository,
            IEmployeeRoleRepository employeeRoleRepository,
            IEventAggregator eventAggregator)
        {
            _employeeRepository = employeeRepository;
            _employeeRoleRepository = employeeRoleRepository;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<AfterDataModelSavedEvent<EmployeeRole>>()
                .Subscribe(AfterEmployeeRoleSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<EmployeeRole>>()
                .Subscribe(AfterEmployeeRoleDeleted);

            EmployeeRoles = new ObservableCollection<EmployeeRoleWrapper>();
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

        /// <summary>
        /// Gets or sets a value indicating whether the database context has changes.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return _hasChanges;
            }

            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        /// <inheritdoc/>
        public async Task LoadAsync(int? employeeId)
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

        private async Task LoadEmployeeRolesAsync()
        {
            var roles = await _employeeRoleRepository.GetAllAsync();
            EmployeeRoles.Clear();
            foreach (var role in roles)
            {
                EmployeeRoles.Add(new EmployeeRoleWrapper(role));
            }
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _employeeRepository.SaveAsync();
            HasChanges = false;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<Employee>>()
                .Publish(new AfterDataModelSavedEventArgs<Employee> { Model = Employee.Model });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Employee != null && !Employee.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _employeeRepository.Remove(Employee.Model);
            await _employeeRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<Employee>>()
                .Publish(new AfterDataModelDeletedEventArgs<Employee> { Model = Employee.Model });
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

        private Employee CreateNewEmployee()
        {
            var employee = new Employee();
            _employeeRepository.Add(employee);
            return employee;
        }
    }
}
