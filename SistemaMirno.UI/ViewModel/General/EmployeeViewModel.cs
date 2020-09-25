// <copyright file="EmployeeViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private EmployeeWrapper _selectedEmployee;

        public EmployeeViewModel(
            IEmployeeRepository employeeRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Empleados", dialogCoordinator)
        {
            _employeeRepository = employeeRepository;

            Employees = new ObservableCollection<EmployeeWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        public ICommand CreateNewCommand { get; }

        public ObservableCollection<EmployeeWrapper> Employees { get; }

        public ICommand OpenDetailCommand { get; }

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

        public override async Task LoadAsync(int? id = null)
        {
            Employees.Clear();

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

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(EmployeeDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedEmployee != null;
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
    }
}
