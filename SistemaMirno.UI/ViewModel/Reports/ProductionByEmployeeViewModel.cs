using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using jsreport.Client;
using LiveCharts;
using LiveCharts.Wpf;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Reports;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Reports
{
    public class ProductionByEmployeeViewModel : ViewModelBase
    {
        private IEmployeeRepository _employeeRepository;

        private readonly PropertyGroupDescription _productName = new PropertyGroupDescription("WorkUnit.Product.Name");

        private Employee _selectedEmployee;

        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;

        private bool _areaPickerEnabled = false;

        public ProductionByEmployeeViewModel(
            IEmployeeRepository employeeRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Reporte de Produccion por Empleado", dialogCoordinator)
        {
            _employeeRepository = employeeRepository;

            Employees = new ObservableCollection<Employee>();
            WorkOrderUnits = new ObservableCollection<WorkOrderUnit>();
            
            WorkUnitsCollection = CollectionViewSource.GetDefaultView(WorkOrderUnits);
            WorkUnitsCollection.GroupDescriptions.Add(_productName);

            MonthlySeriesCollection = new SeriesCollection();
            DailySeriesCollection = new SeriesCollection();

            MonthlyLabels = new[]
            {
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            };
        }

        public SeriesCollection MonthlySeriesCollection { get; }

        public SeriesCollection DailySeriesCollection { get; }
        
        /// <summary>
        /// Gets or sets the selected employee
        /// </summary>
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;

            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
                SelectWorkUnits();
                CalculateMonthlyProduction();
                CalculateDailyProduction();
            }
        }

        public ObservableCollection<Employee> Employees { get; }

        public ObservableCollection<WorkOrderUnit> WorkOrderUnits { get; }

        public ICollectionView WorkUnitsCollection { get; set; }

        /// <summary>
        /// Gets or sets the start date for the report.
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;

            set
            {
                _startDate = value;
                OnPropertyChanged();
                SelectWorkUnits();
            }
        }

        public string[] MonthlyLabels { get; set; }

        /// <summary>
        /// Gets or sets the end date for the report.
        /// </summary>
        public DateTime EndDate
        {
            get => _endDate;

            set
            {
                _endDate = value;
                OnPropertyChanged();
                SelectWorkUnits();
            }
        }

        public bool AreaPickerEnabled
        {
            get => _areaPickerEnabled;

            set
            {
                _areaPickerEnabled = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int? id)
        {
            Employees.Clear();
            var employees = await _employeeRepository.GetAllAsync();

            foreach (var employee in employees)
            {
                Application.Current.Dispatcher.Invoke(() => Employees.Add(employee));
            }
        }

        private async void SelectWorkUnits()
        {
            WorkOrderUnits.Clear();

            if (SelectedEmployee == null)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Visible);

            await Task.Run(() =>
            {
                foreach (var workOrder in SelectedEmployee.WorkOrders)
                {
                    if (workOrder.CreationDateTime.Year < StartDate.Year ||
                        workOrder.CreationDateTime.Month < StartDate.Month ||
                        workOrder.CreationDateTime.Day < StartDate.Day ||
                        workOrder.CreationDateTime.Year > EndDate.Year ||
                        workOrder.CreationDateTime.Month > EndDate.Month ||
                        workOrder.CreationDateTime.Day > EndDate.Day)
                    {
                        continue;
                    }

                    foreach (var workOrderUnit in workOrder.WorkOrderUnits)
                    {
                        Application.Current.Dispatcher.Invoke(() => WorkOrderUnits.Add(workOrderUnit));
                    }
                }
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Hidden;
            });

        }

        private async void CalculateMonthlyProduction()
        {
            long[] monthlyProductions = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (var workOrderUnit in await _employeeRepository.GetThisMonthWorkOrderUnitsFromEmployeeAsync())
            {
                monthlyProductions[workOrderUnit.WorkOrder.CreationDateTime.Month - 1] += workOrderUnit.WorkUnit.Product.ProductionValue;
            }

            var lineSeries = new LineSeries
            {
                Title = "Produccion total",
                Values = new ChartValues<long>(monthlyProductions),
                DataLabels = true,
                LabelPoint = point => $"{point.Y:n0}" + " Gs.",
            };

            MonthlySeriesCollection.Clear();
            MonthlySeriesCollection.Add(lineSeries);
        }

        private void CalculateDailyProduction()
        {
            // Make a list to store the current month daily productions and one to store the cummulative production
            var dailyProductions = new List<long>(new long[DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)]);
            var dailyCumulative = new List<long>(new long[DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)]);

            foreach (var workOrderUnit in WorkOrderUnits)
            {
                if (workOrderUnit.WorkOrder.CreationDateTime.Month == DateTime.Today.Month
                    && workOrderUnit.WorkOrder.CreationDateTime.Year == DateTime.Today.Year)
                {
                    dailyProductions[workOrderUnit.WorkOrder.CreationDateTime.Day - 1] += workOrderUnit.WorkUnit.Product.ProductionValue;
                }
            }

            long cumulative = 0;
            for (var i = 0; i < dailyCumulative.Count; i++)
            {
                cumulative += dailyProductions[i];
                dailyCumulative[i] = cumulative;
            }

            var lineSeriesDaily = new LineSeries
            {
                Title = "Produccion diaria",
                Values = new ChartValues<long>(dailyProductions),
                LabelPoint = point => $"{point.Y:n0}" + " Gs.",
            };

            var lineSeriesCummulative = new LineSeries
            {
                Title = "Produccion acumulada",
                Values = new ChartValues<long>(dailyCumulative),
                LabelPoint = point => $"{point.Y:n0}" + " Gs.",
            };

            DailySeriesCollection.Clear();
            DailySeriesCollection.Add(lineSeriesDaily);
            DailySeriesCollection.Add(lineSeriesCummulative);
        }
    }
}
