﻿// <copyright file="InProcessByWorkAreasViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

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
using jsreport.Client;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Reports;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Reports
{
    public class InProcessByWorkAreasViewModel : ViewModelBase
    {
        private readonly IWorkUnitRepository _workUnitRepository;
        private bool _includeClient = false;
        private bool _includePrice = false;
        private bool _includeResponsible = false;
        private bool _includeSupervisor = false;

        public InProcessByWorkAreasViewModel(
            IWorkUnitRepository workUnitRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Unidad de Trabajo", dialogCoordinator)
        {
            _workUnitRepository = workUnitRepository;

            WorkUnits = new ObservableCollection<WorkUnitWrapper>();

            PrintReportCommand = new DelegateCommand(OnPrintReportExecute);

            WorkUnitsCollection = CollectionViewSource.GetDefaultView(WorkUnits);
            WorkUnitsCollection.GroupDescriptions.Add(new PropertyGroupDescription("Model.CurrentWorkArea.Name"));
            WorkUnitsCollection.GroupDescriptions.Add(new PropertyGroupDescription("Model.Description"));
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

        public ICommand PrintReportCommand { get; }

        public ObservableCollection<WorkUnitWrapper> WorkUnits { get; }

        public ICollectionView WorkUnitsCollection { get; set; }

        public override async Task LoadAsync(int? id)
        {
            WorkUnits.Clear();
            var workUnits = await _workUnitRepository.GetWorkUnitsInProcessAsync();

            foreach (var workUnit in workUnits)
            {
               Application.Current.Dispatcher.Invoke(() => WorkUnits.Add(new WorkUnitWrapper(workUnit)));
            }
        }

        private async void OnPrintReportExecute()
        {
            // Create a new report class to store the report data.
            var inProcessReport = new InProcessReport
            {
                DateTime = DateTime.Now.ToString(),
                WorkAreas = new List<WorkAreaReport>(),
                IncludePrice = _includePrice,
                Total = 0,
            };

            var workAreas = await _workUnitRepository.GetWorkAreasThatReportInProcessAsync();

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
                var workUnits = WorkUnits.Where(w => w.Model.CurrentWorkArea.Name == workArea.Name).ToList();

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
                            if (workUnitReport.Product == workUnit.Model.Product.Name
                                && workUnitReport.Material == workUnit.Model.Material.Name
                                && workUnitReport.Color == workUnit.Model.Color.Name)
                            {
                                workUnitReport.Quantity++;
                                workUnitReport.Price += workUnit.Model.Product.ProductionValue;
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
                                Product = workUnit.Model.Product.Name,
                                Material = workUnit.Model.Material.Name,
                                Color = workUnit.Model.Color.Name,
                                Price = workUnit.Model.Product.ProductionValue,
                                IncludePrice = _includePrice,
                            });
                        }
                    }
                    else
                    {
                        workAreaReport.WorkUnits.Add(new WorkUnitReport
                        {
                            Quantity = 1,
                            Product = workUnit.Model.Product.Name,
                            Material = workUnit.Model.Material.Name,
                            Color = workUnit.Model.Color.Name,
                            Price = workUnit.Model.Product.ProductionValue,
                            IncludePrice = _includePrice,
                        });
                    }

                    // Add the production price of the work unit to the area total.
                    workAreaReport.Total += workUnit.Model.Product.ProductionValue;
                }

                // Add the area total production to the report total.
                inProcessReport.Total += workAreaReport.Total;

                // Add the work area report to the main report
                inProcessReport.WorkAreas.Add(workAreaReport);
            }

            try
            {
                var rs = new ReportingService("http://192.168.1.99:5488", "Mirno", "MirnoReports");

                // Create the json string and send it to the jsreport server for conversion
                var jsonString = JsonConvert.SerializeObject(inProcessReport);
                var report = rs.RenderByNameAsync("processByAreas-main", jsonString).Result;

                Directory.CreateDirectory($"\\\\PC-FABRICA\\SistemaMirno\\Reports");

                // Save the pdf file
                string filename = $"\\\\PC-FABRICA\\SistemaMirno\\Reports\\ProcessReport{DateTime.Now.Ticks}.pdf";
                FileStream stream = new FileStream(filename, FileMode.Create);
                report.Content.CopyTo(stream);
                stream.Close();

                // Open the report with the default application to open pdf files
                ProcessStartInfo info = new ProcessStartInfo
                {
                    Verb = "open",
                    FileName = filename,
                };

                Process.Start(info);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(AggregateException))
                {
                    foreach (var innerEx in ((AggregateException)ex).InnerExceptions)
                    {
                        EventAggregator.GetEvent<ShowDialogEvent>()
                            .Publish(new ShowDialogEventArgs
                            {
                                Message = innerEx.Message,
                                Title = "Error",
                            });
                    }
                }
                else
                {
                    EventAggregator.GetEvent<ShowDialogEvent>()
                        .Publish(new ShowDialogEventArgs
                        {
                            Message = ex.Message,
                            Title = "Error",
                        });
                }
            }
        }
    }
}
