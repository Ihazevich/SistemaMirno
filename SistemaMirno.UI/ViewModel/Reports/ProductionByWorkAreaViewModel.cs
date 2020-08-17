using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using jsreport.Client;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Reports;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Reports
{
    public class ProductionByWorkAreaViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IWorkAreaRepository _workAreaRepository;

        private PropertyGroupDescription _colorName = new PropertyGroupDescription("WorkUnit.Color.Name");
        private PropertyGroupDescription _materialName = new PropertyGroupDescription("WorkUnit.Material.Name");
        private PropertyGroupDescription _productName = new PropertyGroupDescription("WorkUnit.Product.Name");

        private WorkAreaWrapper _selectedWorkArea;

        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;

        private bool _areaPickerEnabled = false;

        public ProductionByWorkAreaViewModel(
                    IWorkAreaRepository workAreaRepository,
                    IEventAggregator eventAggregator)
        {
            _workAreaRepository = workAreaRepository;
            _eventAggregator = eventAggregator;

            WorkAreas = new ObservableCollection<WorkAreaWrapper>();
            WorkOrderUnits = new ObservableCollection<WorkOrderUnitWrapper>();

            PrintReportCommand = new DelegateCommand(OnPrintReportExecute);

            WorkUnitsCollection = CollectionViewSource.GetDefaultView(WorkOrderUnits);
            WorkUnitsCollection.GroupDescriptions.Add(_productName);
            WorkUnitsCollection.GroupDescriptions.Add(_materialName);
            WorkUnitsCollection.GroupDescriptions.Add(_colorName);

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

        public SeriesCollection MonthlySeriesCollection { get; set; }

        public SeriesCollection DailySeriesCollection { get; set; }

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

        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; set; }

        public ObservableCollection<WorkOrderUnitWrapper> WorkOrderUnits { get; set; }

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

        public override async Task LoadAsync(int? workAreaId)
        {
            WorkAreas.Clear();
            var workAreas = await _workAreaRepository.GetAllAsync();

            foreach (var workArea in workAreas)
            {
                WorkAreas.Add(new WorkAreaWrapper(workArea));
            }
        }

        private async void OnPrintReportExecute()
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
                var model = workOrderUnit.WorkUnit;

                // If there is already a work unit in the area report, check to group the similar ones
                // else just add the Work Unit.
                if (productionReport.WorkUnits.Count > 0)
                {
                    bool found = false;
                    foreach (var workUnitReport in productionReport.WorkUnits)
                    {
                        // If there is a work unit in the report that has the same properties, just add to the quantity.
                        if (workUnitReport.Product == workOrderUnit.WorkUnit.Product.Name
                            && workUnitReport.Material == workOrderUnit.WorkUnit.Material.Name
                            && workUnitReport.Color == workOrderUnit.WorkUnit.Color.Name)
                        {
                            workUnitReport.Quantity++;
                            workUnitReport.Price += workOrderUnit.WorkUnit.Product.ProductionPrice;
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
                            Product = workOrderUnit.WorkUnit.Product.Name,
                            Material = workOrderUnit.WorkUnit.Material.Name,
                            Color = workOrderUnit.WorkUnit.Color.Name,
                            Price = workOrderUnit.WorkUnit.Product.ProductionPrice,
                        });
                    }
                }
                else
                {
                    productionReport.WorkUnits.Add(new WorkUnitReport
                    {
                        Quantity = 1,
                        Product = workOrderUnit.WorkUnit.Product.Name,
                        Material = workOrderUnit.WorkUnit.Material.Name,
                        Color = workOrderUnit.WorkUnit.Color.Name,
                        Price = workOrderUnit.WorkUnit.Product.ProductionPrice,
                    });
                }

                // Add the production price of the work unit to the report total.
                productionReport.Total += workOrderUnit.WorkUnit.Product.ProductionPrice;

            }

            // Create the json string and send it to the jsreport server for conversion
            var rs = new ReportingService("http://127.0.0.1:5488", "admin", "mirno");
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
            if (SelectedWorkArea != null)
            {
                if (SelectedWorkArea.IncomingWorkOrders != null)
                {
                    foreach (WorkOrder workOrder in SelectedWorkArea.IncomingWorkOrders)
                    {
                        if (workOrder.StartTime > StartDate && workOrder.StartTime < EndDate)
                        {
                            foreach (WorkOrderUnit workOrderUnit in workOrder.WorkOrderUnits)
                            {
                                WorkOrderUnits.Add(new WorkOrderUnitWrapper(workOrderUnit));
                            }
                        }
                    }

                    CalculateMonthlyProduction();
                    CalculateDailyProduction();
                }
            }
        }

        private bool ValidateDates()
        {
            bool areDatesValid = false;

            if (StartDate != null && EndDate != null)
            {
                if (StartDate.Date <= DateTime.Today.Date && EndDate.Date >= StartDate.Date)
                {
                    areDatesValid = true;
                }
            }

            AreaPickerEnabled = areDatesValid;
            return areDatesValid;
        }

        private void CalculateMonthlyProduction()
        {
            int[] monthlyProductions = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (var workOrderUnit in WorkOrderUnits)
            {
                monthlyProductions[workOrderUnit.WorkOrder.StartTime.Month - 1] += workOrderUnit.WorkUnit.Product.ProductionPrice;
            }

            LineSeries lineSeries = new LineSeries
            {
                Title = "Produccion total",
                Values = new ChartValues<int>(monthlyProductions),
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
                if (workOrderUnit.WorkOrder.StartTime.Month == DateTime.Today.Month
                    && workOrderUnit.WorkOrder.StartTime.Year == DateTime.Today.Year)
                {
                    dailyProductions[workOrderUnit.WorkOrder.StartTime.Day - 1] += workOrderUnit.WorkUnit.Product.ProductionPrice;
                }
            }

            long cummulative = 0;
            for (int i = 0; i < dailyCummulative.Count; i++)
            {
                cummulative += dailyProductions[i];
                dailyCummulative[i] = cummulative;
            }

            LineSeries lineSeriesDaily = new LineSeries
            {
                Title = "Produccion diaria",
                Values = new ChartValues<long>(dailyProductions),
                LabelPoint = point => string.Format("{0:n0}", point.Y) + " Gs.",
            };

            LineSeries lineSeriesCummulative = new LineSeries
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
