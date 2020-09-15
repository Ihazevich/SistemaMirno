using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class EmployeeDetailViewModel : DetailViewModelBase
    {
        private IEmployeeRepository _employeeRepository;
        private EmployeeWrapper _employee;
        private BranchWrapper _selectedBranch;
        private RoleWrapper _selectedAddRole;
        private RoleWrapper _selectedRemoveRole;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchDetailViewModel"/> class.
        /// </summary>
        /// <param name="employeeRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public EmployeeDetailViewModel(
            IEmployeeRepository employeeRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Empleado", dialogCoordinator)
        {
            _employeeRepository = employeeRepository;
            Branches = new ObservableCollection<BranchWrapper>();
            Roles = new ObservableCollection<RoleWrapper>();
            EmployeeRoles = new ObservableCollection<RoleWrapper>();

            AddRoleCommand = new DelegateCommand(OnAddRoleExecute, OnAddRoleCanExecute);
            RemoveRoleCommand = new DelegateCommand(OnRemoveRoleExecute, OnRemoveRoleCanExecute);
            SelectFileCommand =new DelegateCommand(OnSelectFileExecute);
        }

        private void OnRemoveRoleExecute()
        {
            Employee.Model.Roles.Remove(SelectedRemoveRole.Model);
            EmployeeRoles.Remove(SelectedRemoveRole);
            HasChanges = true;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveRoleCanExecute()
        {
            return SelectedRemoveRole != null;
        }

        private void OnAddRoleExecute()
        {
            Employee.Model.Roles.Add(SelectedAddRole.Model);
            EmployeeRoles.Add(SelectedAddRole);
            HasChanges = true;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddRoleCanExecute()
        {
            return SelectedAddRole != null;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public EmployeeWrapper Employee
        {
            get => _employee;

            set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }

        public BranchWrapper SelectedBranch
        {
            get => _selectedBranch;

            set
            {
                _selectedBranch = value;
                OnPropertyChanged();
                if (_selectedBranch != null)
                {
                    NewBranchSelected(_selectedBranch.Id);
                }
            }
        }

        private async Task NewBranchSelected(int id)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsEnabled = false;
                ProgressVisibility = Visibility.Visible;
            });
            var roles = await _employeeRepository.GetAllRolesFromBranchAsync(id);
            Roles.Clear();
            foreach (var role in roles)
            {
                Application.Current.Dispatcher.Invoke(() => Roles.Add(new RoleWrapper(role)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsEnabled = true;
                ProgressVisibility = Visibility.Collapsed;
            });
        }

        public RoleWrapper SelectedAddRole
        {
            get => _selectedAddRole;

            set
            {
                _selectedAddRole = value;
                OnPropertyChanged();
                ((DelegateCommand)AddRoleCommand).RaiseCanExecuteChanged();
            }
        }

        public RoleWrapper SelectedRemoveRole
        {
            get => _selectedRemoveRole;

            set
            {
                _selectedRemoveRole = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveRoleCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<BranchWrapper> Branches { get; }

        public ObservableCollection<RoleWrapper> Roles { get; }

        public ObservableCollection<RoleWrapper> EmployeeRoles { get; }

        public ICommand AddRoleCommand { get; }

        public ICommand RemoveRoleCommand { get; }

        public ICommand SelectFileCommand { get; }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _employeeRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Employee = new EmployeeWrapper(model);
                Employee.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            foreach (var role in Employee.Model.Roles)
            {
                Application.Current.Dispatcher.Invoke(() => EmployeeRoles.Add(new RoleWrapper(role)));
            }

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();
            if (IsNew)
            {
                Employee.SalaryExtraHoursBonus = 0;
                Employee.SalaryNormalHoursBonus = 0;
                Employee.SalaryProductionBonus = 0;
                Employee.SalarySalesBonus = 0;
                Employee.SalaryWorkOrderBonus = 0;
                Employee.TotalSalary = Employee.BaseSalary + Employee.SalaryOtherBonus;
                await _employeeRepository.AddAsync(Employee.Model);
            }
            else
            {
                await _employeeRepository.SaveAsync(Employee.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(EmployeeViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Employee);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            base.OnDeleteExecute();
            await _employeeRepository.DeleteAsync(Employee.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(EmployeeViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            base.OnCancelExecute();
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(EmployeeViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _employeeRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Employee.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadBranches();

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                Employee = new EmployeeWrapper();
                Employee.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                Employee.FirstName = string.Empty;
                Employee.LastName = string.Empty;
                Employee.DocumentNumber = string.Empty;
                Employee.BirthDate = DateTime.Today.Subtract(TimeSpan.FromDays(365 * 18));
                Employee.Age = 18;
                Employee.Address = string.Empty;
                Employee.PhoneNumber = string.Empty;
                Employee.BaseSalary = 0;
                Employee.SalaryOtherBonus = 0;
                Employee.PricePerExtraHour = 0;
                Employee.PricePerNormalHour = 0;
                Employee.ContractStartDate = DateTime.Today;
                Employee.ContractFile = string.Empty;
                Employee.IsRegisteredInIps = false;
                Employee.IpsStartDate = null;
                Employee.Terminated = false;
                Employee.TerminationDate = null;
                Employee.UserId = null;

                Employee.SalaryExtraHoursBonus = 0;
                Employee.SalaryNormalHoursBonus = 0;
                Employee.SalaryProductionBonus = 0;
                Employee.SalarySalesBonus = 0;
                Employee.SalaryWorkOrderBonus = 0;
                Employee.TotalSalary = Employee.BaseSalary + Employee.SalaryOtherBonus;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadBranches()
        {
            var branches = await _employeeRepository.GetAllBranchesAsync();

            foreach (var branch in branches)
            {
                Application.Current.Dispatcher.Invoke(() => Branches.Add(new BranchWrapper(branch)));
            }
        }

        private void OnSelectFileExecute()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "Archivos PDF(.pdf)|*.pdf"; // Filter files by extension

            bool? result = dlg.ShowDialog();
            // Process open file dialog box results
            if (result == true)
            {
                // Save filename
                Application.Current.Dispatcher.Invoke(() => Employee.ContractFile = dlg.FileName);
            }
        }
    }
}
