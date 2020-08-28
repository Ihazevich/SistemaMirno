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
    public class EmployeeViewModel : ViewModelBase, IEmployeeViewModel
    {
        private IEmployeeRepository _employeeRepository;
        private Func<IEmployeeRepository> _employeeRepositoryCreator;
        private EmployeeWrapper _selectedEmployee;
        private IEmployeeDetailViewModel _employeeDetailViewModel;
        private Func<IEmployeeDetailViewModel> _employeeDetailViewModelCreator;

        public EmployeeViewModel(
            Func<IEmployeeDetailViewModel> employeeDetailViewModelCreator,
            Func<IEmployeeRepository> employeeRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Empleados", dialogCoordinator)
        {
            _employeeDetailViewModelCreator = employeeDetailViewModelCreator;
            _employeeRepositoryCreator = employeeRepositoryCreator;

            Employees = new ObservableCollection<EmployeeWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedEmployee.Id,
                    ViewModel = nameof(EmployeeDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedEmployee != null;
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(EmployeeDetailViewModel),
                });
        }

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

        public ObservableCollection<EmployeeWrapper> Employees { get; set; }

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
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Employees.Clear();
            _employeeRepository = _employeeRepositoryCreator();

            var employees = await _employeeRepository.GetAllAsync();

            foreach (var employee in employees)
            {
                Application.Current.Dispatcher.Invoke(() => Employees.Add(new EmployeeWrapper(employee)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }
    }
}
