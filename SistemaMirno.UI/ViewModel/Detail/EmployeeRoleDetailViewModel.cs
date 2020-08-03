using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class EmployeeRoleDetailViewModel : DetailViewModelBase, IEmployeeRoleDetailViewModel
    {

        private IEmployeeRoleRepository _employeeRoleRepository;
        private EmployeeRoleWrapper _employeRolee;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRoleDetailViewModel"/> class.
        /// </summary>
        /// <param name="employeeRoleRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public EmployeeRoleDetailViewModel(
            IEmployeeRoleRepository employeeRoleRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _employeeRoleRepository = employeeRoleRepository;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public EmployeeRoleWrapper EmployeeRole
        {
            get
            {
                return _employeRolee;
            }

            set
            {
                _employeRolee = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? employeeRoleId)
        {
            var role = employeeRoleId.HasValue
                ? await _employeeRoleRepository.GetByIdAsync(employeeRoleId.Value)
                : CreateNewEmployeeRole();

            EmployeeRole = new EmployeeRoleWrapper(role);
            EmployeeRole.PropertyChanged += Color_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (role.Id == 0)
            {
                // This triggers the validation.
                EmployeeRole.Name = string.Empty;
            }
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _employeeRoleRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(EmployeeRole.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return EmployeeRole != null && !EmployeeRole.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _employeeRoleRepository.Remove(EmployeeRole.Model);
            await _employeeRoleRepository.SaveAsync();
            RaiseDataModelDeletedEvent(EmployeeRole.Model);
        }

        private void Color_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _employeeRoleRepository.HasChanges();
            }

            if (e.PropertyName == nameof(EmployeeRole.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private EmployeeRole CreateNewEmployeeRole()
        {
            var role = new EmployeeRole();
            _employeeRoleRepository.Add(role);
            return role;
        }
    }
}
