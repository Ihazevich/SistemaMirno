using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using jsreport.Binary;
using jsreport.Client;
using jsreport.Local;
using LiveCharts;
using LiveCharts.Wpf;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Reports;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Reports
{
    public class ProductionByWorkAreaViewModel : ViewModelBase
    {
        private IWorkAreaRepository _workAreaRepository;

        private readonly PropertyGroupDescription _productName = new PropertyGroupDescription("Model.WorkUnit.Product.Name");

        private WorkAreaWrapper _selectedWorkArea;

        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;

        private bool _areaPickerEnabled = false;

        public ProductionByWorkAreaViewModel(
            IWorkAreaRepository workAreaRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Reporte de Produccion por Area", dialogCoordinator)
        {
            _workAreaRepository = workAreaRepository;

            WorkAreas = new ObservableCollection<WorkAreaWrapper>();
            WorkOrderUnits = new ObservableCollection<WorkOrderUnitWrapper>();

            PrintReportCommand = new DelegateCommand(OnPrintReportExecute);

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

        public ICommand PrintReportCommand { get; }

        /// <summary>
        /// Gets or sets the selected work area.
        /// </summary>
        public WorkAreaWrapper SelectedWorkArea
        {
            get => _selectedWorkArea;

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
                SelectWorkUnits();
            }
        }

        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; }

        public ObservableCollection<WorkOrderUnitWrapper> WorkOrderUnits { get; }

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
                if (ValidateDates())
                {
                    SelectWorkUnits();
                }
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
                if (ValidateDates())
                {
                    SelectWorkUnits();
                }
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
            WorkAreas.Clear();
            var workAreas = await _workAreaRepository.GetAllAsync();

            foreach (var workArea in workAreas)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreas.Add(new WorkAreaWrapper(workArea)));
            }
        }

        private void OnPrintReportExecute()
        {
            // Create a new report class to store the report data.
            var productionReport = new ProductionReport
            {
                FromDate = StartDate.Date.ToShortDateString(),
                ToDate = EndDate.Date.ToShortDateString(),
                WorkArea = SelectedWorkArea.Name,
                Total = 0,
                WorkUnits = new List<WorkUnitReport>(),
            };

            foreach (var workOrderUnit in WorkOrderUnits)
            {
                var model = workOrderUnit.Model.WorkUnit;

                // If there is already a work unit in the area report, check to group the similar ones
                // else just add the Work Unit.
                if (productionReport.WorkUnits.Count > 0)
                {
                    bool found = false;
                    foreach (var workUnitReport in productionReport.WorkUnits)
                    {
                        // If there is a work unit in the report that has the same properties, just add to the quantity.
                        if (workUnitReport.Product == workOrderUnit.Model.WorkUnit.Product.Name
                            && workUnitReport.Material == workOrderUnit.Model.WorkUnit.Material.Name
                            && workUnitReport.Color == workOrderUnit.Model.WorkUnit.Color.Name)
                        {
                            workUnitReport.Quantity++;
                            workUnitReport.Price += workOrderUnit.Model.WorkUnit.Product.ProductionValue;
                            found = true;
                            break;
                        }
                    }

                    // If there wasn't any work unit with the same properties, add the work unit to the report.
                    if (!found)
                    {
                        productionReport.WorkUnits.Add(new WorkUnitReport
                        {
                            Quantity = 1,
                            Product = workOrderUnit.Model.WorkUnit.Product.Name,
                            Material = workOrderUnit.Model.WorkUnit.Material.Name,
                            Color = workOrderUnit.Model.WorkUnit.Color.Name,
                            Price = workOrderUnit.Model.WorkUnit.Product.ProductionValue,
                        });
                    }
                }
                else
                {
                    productionReport.WorkUnits.Add(new WorkUnitReport
                    {
                        Quantity = 1,
                        Product = workOrderUnit.Model.WorkUnit.Product.Name,
                        Material = workOrderUnit.Model.WorkUnit.Material.Name,
                        Color = workOrderUnit.Model.WorkUnit.Color.Name,
                        Price = workOrderUnit.Model.WorkUnit.Product.ProductionValue,
                    });
                }

                // Add the production price of the work unit to the report total.
                productionReport.Total += workOrderUnit.Model.WorkUnit.Product.ProductionValue;

            }

            // Create the json string and send it to the jsreport server for conversion

            var rs = new LocalReporting()
                .UseBinary(JsReportBinary.GetBinary())
                .Configure(cfg => cfg.FileSystemStore().BaseUrlAsWorkingDirectory())
                .AsUtility()
                .Create();

            var jsonString = JsonConvert.SerializeObject(productionReport);
            var report = rs.RenderByNameAsync("productionByArea-main", jsonString).Result;

            // Save the pdf file
            string filename = $"{Directory.GetCurrentDirectory()}\\Reports\\ProcessReport{DateTime.Now.Ticks}.pdf";
            FileStream stream = new FileStream(filename, FileMode.Create);
            report.Content.CopyTo(stream);
            stream.Close();

            // Open the report with the default application to open pdf files
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "open";
            info.FileName = filename;

            Process.Start(info);
        }

        private async void SelectWorkUnits()
        {
            WorkOrderUnits.Clear();
            if (SelectedWorkArea?.Model.IncomingWorkOrders == null)
            {
                return;
            }

            foreach (var workOrder in SelectedWorkArea.Model.IncomingWorkOrders)
            {
                if (workOrder.CreationDateTime <= StartDate || workOrder.CreationDateTime >= EndDate) continue;
                foreach (var workOrderUnit in workOrder.WorkOrderUnits)
                {
                    WorkOrderUnits.Add(new WorkOrderUnitWrapper(workOrderUnit));
                }
            }

            CalculateMonthlyProduction();
            CalculateDailyProduction();
        }

        private bool ValidateDates()
        {
            bool areDatesValid = StartDate.Date <= DateTime.Today.Date && EndDate.Date >= StartDate.Date;

            AreaPickerEnabled = areDatesValid;
            return areDatesValid;
        }

        private void CalculateMonthlyProduction()
        {
            long[] monthlyProductions = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (var workOrderUnit in WorkOrderUnits)
            {
                monthlyProductions[workOrderUnit.Model.WorkOrder.CreationDateTime.Month - 1] += workOrderUnit.Model.WorkUnit.Product.ProductionValue;
            }

            LineSeries lineSeries = new LineSeries
            {
                Title = "Produccion total",
                Values = new ChartValues<long>(monthlyProductions),
                DataLabels = true,
                LabelPoint = point => string.Format("{0:n0}", point.Y) + " Gs.",
            };

            MonthlySeriesCollection.Clear();
            MonthlySeriesCollection.Add(lineSeries);
        }

        private void CalculateDailyProduction()
        {
            // Make a list to store the current month daily productions and one to store the cummulative production
            var dailyProductions = new List<long>(new long[DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)]);
            var dailyCummulative = new List<long>(new long[DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)]);

            foreach (var workOrderUnit in WorkOrderUnits)
            {
                if (workOrderUnit.Model.WorkOrder.CreationDateTime.Month == DateTime.Today.Month
                    && workOrderUnit.Model.WorkOrder.CreationDateTime.Year == DateTime.Today.Year)
                {
                    dailyProductions[workOrderUnit.Model.WorkOrder.CreationDateTime.Day - 1] += workOrderUnit.Model.WorkUnit.Product.ProductionValue;
                }
            }

            long cummulative = 0;
            for (var i = 0; i < dailyCummulative.Count; i++)
            {
                cummulative += dailyProductions[i];
                dailyCummulative[i] = cummulative;
            }

            var lineSeriesDaily = new LineSeries
            {
                Title = "Produccion diaria",
                Values = new ChartValues<long>(dailyProductions),
                LabelPoint = point => string.Format("{0:n0}", point.Y) + " Gs.",
            };

            var lineSeriesCummulative = new LineSeries
            {
                Title = "Produccion acumulada",
                Values = new ChartValues<long>(dailyCummulative),
                LabelPoint = point => string.Format("{0:n0}", point.Y) + " Gs.",
            };

            DailySeriesCollection.Clear();
            DailySeriesCollection.Add(lineSeriesDaily);
            DailySeriesCollection.Add(lineSeriesCummulative);
        }
    }
}