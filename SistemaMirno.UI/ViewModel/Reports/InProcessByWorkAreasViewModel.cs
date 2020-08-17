using jsreport.Client;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Reports;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel.Reports
{
    public class InProcessByWorkAreasViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IWorkUnitRepository _workUnitRepository;

        private PropertyGroupDescription _workAreaName = new PropertyGroupDescription("WorkArea.Name");
        private PropertyGroupDescription _colorName = new PropertyGroupDescription("Color.Name");
        private PropertyGroupDescription _materialName = new PropertyGroupDescription("Material.Name");
        private PropertyGroupDescription _productName = new PropertyGroupDescription("Product.Name");

        private bool _includePrice = false;
        private bool _includeResponsible = false;
        private bool _includeSupervisor = false;
        private bool _includeClient = false;

        public InProcessByWorkAreasViewModel(IWorkUnitRepository workUnitRepository,
                    IEventAggregator eventAggregator)
        {
            _workUnitRepository = workUnitRepository;
            _eventAggregator = eventAggregator;

            WorkUnits = new ObservableCollection<WorkUnitWrapper>();

            PrintReportCommand = new DelegateCommand(OnPrintReportExecute);

            WorkUnitsCollection = CollectionViewSource.GetDefaultView(WorkUnits);
            WorkUnitsCollection.GroupDescriptions.Add(_workAreaName);
            WorkUnitsCollection.GroupDescriptions.Add(_productName);
            WorkUnitsCollection.GroupDescriptions.Add(_materialName);
            WorkUnitsCollection.GroupDescriptions.Add(_colorName);
        }

        public ICommand PrintReportCommand { get; }

        public bool IncludePrice
        {
            get => _includePrice;

            set
            {
                _includePrice = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeResponsible
        {
            get => _includeResponsible;

            set
            {
                _includeResponsible = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeSupervisor
        {
            get => _includeSupervisor;

            set
            {
                _includeSupervisor = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeCLient
        {
            get => _includeClient;

            set
            {
                _includeClient = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<WorkUnitWrapper> WorkUnits { get; set; }

        public ICollectionView WorkUnitsCollection { get; set; }

        public override async Task LoadAsync(int? workAreaId)
        {
            WorkUnits.Clear();
            var workUnits = await _workUnitRepository.GetWorkUnitsInProcessAsync();

            foreach (var workUnit in workUnits)
            {
                WorkUnits.Add(new WorkUnitWrapper(workUnit));
            }
        }

        private async void OnPrintReportExecute()
        {
            // Create a new report class to store the report data.
            var inProcessReport = new InProcessReport
            {
                Datetime = DateTime.Now.ToString(),
                WorkAreas = new List<WorkAreaReport>(),
                IncludePrice = _includePrice,
                Total = 0,
            };

            var workAreas = await _workUnitRepository.GetWorkAreasThatReportInProcess();

            foreach (var workArea in workAreas)
            {
                var workAreaReport = new WorkAreaReport
                {
                    Name = workArea.Name,
                    WorkUnits = new List<WorkUnitReport>(),
                    IncludePrice = _includePrice,
                    Total = 0,
                };

                // Select all work units in the current work area
                var workUnits = WorkUnits.Where(w => w.WorkArea.Name == workArea.Name).ToList();

                // Create the reports for each Work Unit in the Work Area
                foreach (var workUnit in workUnits)
                {
                    var model = workUnit.Model;

                    // If there is already a work unit in the area report, check to group the similar ones
                    // else just add the Work Unit.
                    if (workAreaReport.WorkUnits.Count > 0)
                    {
                        bool found = false;
                        foreach (var workUnitReport in workAreaReport.WorkUnits)
                        {
                            // If there is a work unit in the report that has the same properties, just add to the quantity.
                            if (workUnitReport.Product == workUnit.Product.Name
                                && workUnitReport.Material == workUnit.Material.Name
                                && workUnitReport.Color == workUnit.Color.Name)
                            {
                                workUnitReport.Quantity++;
                                found = true;
                                break;
                            }
                        }

                        // If there wasn't any work unit with the same properties, add the work unit to the report.
                        if (!found)
                        {
                            workAreaReport.WorkUnits.Add(new WorkUnitReport
                            {
                                Quantity = 1,
                                Product = workUnit.Product.Name,
                                Material = workUnit.Material.Name,
                                Color = workUnit.Color.Name,
                                Price = workUnit.Product.ProductionPrice,
                                IncludePrice = _includePrice,
                            });
                        }
                    }
                    else
                    {
                        workAreaReport.WorkUnits.Add(new WorkUnitReport
                        {
                            Quantity = 1,
                            Product = workUnit.Product.Name,
                            Material = workUnit.Material.Name,
                            Color = workUnit.Color.Name,
                            Price = workUnit.Product.ProductionPrice,
                            IncludePrice = _includePrice,
                        });
                    }

                    // Add the production price of the work unit to the area total.
                    workAreaReport.Total += workUnit.Product.ProductionPrice;
                }

                // Add the area total production to the report total.
                inProcessReport.Total += workAreaReport.Total;

                // Add the work area report to the main report
                inProcessReport.WorkAreas.Add(workAreaReport);
            }

            // Create the json string and send it to the jsreport server for conversion
            var rs = new ReportingService("http://127.0.0.1:5488", "admin", "mirno");
            var jsonString = JsonConvert.SerializeObject(inProcessReport);
            var report = rs.RenderByNameAsync("processByAreas-main", jsonString).Result;

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
    }
}
