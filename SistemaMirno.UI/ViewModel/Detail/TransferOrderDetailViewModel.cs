using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using jsreport.Client;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Reports;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class TransferOrderDetailViewModel : DetailViewModelBase
    {
        private ITransferOrderRepository _transferOrderRepository;
        private TransferOrderWrapper _transferOrder;
        private WorkUnit _selectedTransferWorkUnit;
        private WorkUnit _selectedExistingWorkUnit;
        private Employee _selectedResponsible;
        private Branch _selectedDestinationBranch;
        private readonly PropertyGroupDescription _workArea = new PropertyGroupDescription("CurrentWorkArea.Name");
        private readonly PropertyGroupDescription _description = new PropertyGroupDescription("Description");

        public TransferOrderDetailViewModel(
            ITransferOrderRepository transferOrderRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Orden de Traslado", dialogCoordinator)
        {
            _transferOrderRepository = transferOrderRepository;

            WorkAreas = new ObservableCollection<WorkArea>();
            Branches = new ObservableCollection<Branch>();
            Vehicles = new ObservableCollection<Vehicle>();
            Responsibles = new ObservableCollection<Employee>();

            ExistingWorkUnits = new ObservableCollection<WorkUnit>();
            TransferWorkUnits = new ObservableCollection<WorkUnit>();

            ExistingWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(ExistingWorkUnits);
            ExistingWorkUnitsCollectionView.GroupDescriptions.Add(_workArea);
            ExistingWorkUnitsCollectionView.GroupDescriptions.Add(_description);
            TransferWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(TransferWorkUnits);
            TransferWorkUnitsCollectionView.GroupDescriptions.Add(_workArea);
            TransferWorkUnitsCollectionView.GroupDescriptions.Add(_description);

            AddWorkUnitCommand = new DelegateCommand(OnAddWorkUnitExecute, OnAddWorkUnitCanExecute);
            RemoveWorkUnitCommand = new DelegateCommand(OnRemoveWorkUnitExecute, OnRemoveWorkUnitCanExecute);
        }

        private bool OnAddWorkUnitCanExecute()
        {
            return SelectedExistingWorkUnit != null;
        }

        private void OnAddWorkUnitExecute()
        {
            while (SelectedExistingWorkUnit != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TransferWorkUnits.Add(SelectedExistingWorkUnit);
                    ExistingWorkUnits.Remove(SelectedExistingWorkUnit);
                });
            }
        }

        private bool OnRemoveWorkUnitCanExecute()
        {
            return SelectedTransferWorkUnit != null;
        }

        private void OnRemoveWorkUnitExecute()
        {
            while (SelectedTransferWorkUnit != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ExistingWorkUnits.Add(SelectedTransferWorkUnit);
                    TransferWorkUnits.Remove(SelectedTransferWorkUnit);
                });
            }
        }

        public ICommand AddWorkUnitCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

        public ICollectionView ExistingWorkUnitsCollectionView { get; }

        public ICollectionView TransferWorkUnitsCollectionView { get; }

        public ObservableCollection<WorkUnit> TransferWorkUnits { get; }

        public ObservableCollection<WorkUnit> ExistingWorkUnits { get; }

        public ObservableCollection<WorkArea> WorkAreas { get; }

        public ObservableCollection<Branch> Branches { get; }

        public ObservableCollection<Vehicle> Vehicles { get; }

        public ObservableCollection<Employee> Responsibles { get; }

        public TransferOrderWrapper TransferOrder
        {
            get => _transferOrder;

            set
            {
                _transferOrder = value;
                OnPropertyChanged();
            }
        }

        public WorkUnit SelectedExistingWorkUnit
        {
            get => _selectedExistingWorkUnit;

            set
            {
                _selectedExistingWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)AddWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkUnit SelectedTransferWorkUnit
        {
            get => _selectedTransferWorkUnit;

            set
            {
                _selectedTransferWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public Employee SelectedResponsible
        {
            get => _selectedResponsible;

            set
            {
                _selectedResponsible = value;
                OnPropertyChanged();
            }
        }

        public Branch SelectedDestinationBranch
        {
            get => _selectedDestinationBranch;

            set
            {
                _selectedDestinationBranch = value;
                OnPropertyChanged();
                if (_selectedDestinationBranch != null)
                {
                    LoadWorkUnitsAsync().ConfigureAwait(false);
                }
            }
        }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _transferOrderRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                TransferOrder = new TransferOrderWrapper(model);
                TransferOrder.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            foreach (var workUnit in TransferWorkUnits)
            {
                workUnit.Moving = true;

                var transferUnit = new TransferUnit
                {
                    WorkUnit = workUnit,
                    WorkUnitId = workUnit.Id,
                };

                TransferOrder.Model.TransferUnits.Add(transferUnit);
            }

            TransferOrder.Date = DateTime.Now;

            if (IsNew)
            {
                await _transferOrderRepository.AddAsync(TransferOrder.Model);
            }
            else
            {
                await _transferOrderRepository.SaveAsync(TransferOrder.Model);
            }

            try
            {
                await CreateDeliveryOrderReport();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Title = "Error",
                        Message = e.Message,
                    });
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ColorViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(TransferOrder);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            base.OnDeleteExecute();
            await _transferOrderRepository.DeleteAsync(TransferOrder.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ColorViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            base.OnCancelExecute();
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ColorViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _transferOrderRepository.HasChanges();
            }

            if (e.PropertyName == nameof(TransferOrder.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadBranchesAsync();
            await LoadVehiclesAsync();
            await LoadResponsiblesAsync();

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                TransferOrder = new TransferOrderWrapper();
                TransferOrder.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                TransferOrder.Date = DateTime.Today;
                TransferOrder.FromBranchId = SessionInfo.Branch.Id;
                TransferOrder.ToBranchId = 0;
                TransferOrder.VehicleId = 0;
                TransferOrder.ResponsibleId = 0;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadWorkUnitsAsync()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Visible;
                IsEnabled = false;
            });

            var workUnits = await Task.Run(() =>
                _transferOrderRepository.GetAllWorkUnitsAvailableForTransferAsync(SelectedDestinationBranch.Id));

            if (workUnits != null)
            {
                foreach (var workUnit in workUnits)
                {
                    Application.Current.Dispatcher.Invoke(() => ExistingWorkUnits.Add(workUnit));
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                IsEnabled = true;
            });
        }

        private async Task LoadResponsiblesAsync()
        {
            var responsibles = await _transferOrderRepository.GetAllLogisticResponsiblesAsync();

            foreach (var responsible in responsibles)
            {
                Application.Current.Dispatcher.Invoke(() => Responsibles.Add(responsible));
            }
        }

        private async Task LoadVehiclesAsync()
        {
            var vehicles = await _transferOrderRepository.GetAllVehiclesAsync();

            foreach (var vehicle in vehicles)
            {
                Application.Current.Dispatcher.Invoke(() => Vehicles.Add(vehicle));
            }
        }

        private async Task LoadBranchesAsync()
        {
            var branches = await _transferOrderRepository.GetAllBranchesNotCurrentAsync(SessionInfo.Branch.Id);

            foreach (var branch in branches)
            {
                Application.Current.Dispatcher.Invoke(() => Branches.Add(branch));
            }
        }


        private async Task CreateDeliveryOrderReport()
        {
            // Create a new report class with the Work Order data.
            var deliveryOrderReport = new DeliveryOrderReport
            {
                Date = DateTime.Today.Date.ToString("d"),
                Responsible = SelectedResponsible.FullName,
                IsTransfer = true,
                Branch = SelectedDestinationBranch.Name,
            };

            foreach (var workArea in TransferWorkUnits.GroupBy(w=>w.CurrentWorkArea).Select(g => g.Key).ToList())
            {
                var newWorkArea = new ClientReport
                {
                    Name = workArea.Name,
                };

                // Select all work units in the current workArea
                var workUnits = TransferWorkUnits.Where(w => w.CurrentWorkAreaId == workArea.Id).ToList();

                // Create the reports for each Work Unit in the Work Area
                foreach (var workUnit in workUnits)
                {
                    var client = workUnit.Requisition != null ? workUnit.Requisition.Client.FullName :
                        workUnit.Sale != null ? workUnit.Sale.Client.FullName : "Stock";

                    // If there is already a work unit in the client report, check to group the similar ones
                    // else just add the Work Unit.
                    if (newWorkArea.WorkUnits.Count > 0)
                    {
                        var found = false;
                        foreach (var workUnitReport in newWorkArea.WorkUnits)
                        {
                            // If there is a work unit in the report that has the same properties, just add to the quantity.
                            if (workUnitReport.Product == workUnit.Product.Name
                                && workUnitReport.Material == workUnit.Material.Name
                                && workUnitReport.Color == workUnit.Color.Name
                                && workUnitReport.CurrentWorkArea == client)
                            {
                                workUnitReport.Quantity++;
                                found = true;
                                break;
                            }
                        }

                        // If there wasn't any work unit with the same properties, add the work unit to the report.
                        if (!found)
                        {
                            newWorkArea.WorkUnits.Add(new WorkUnitReport
                            {
                                Quantity = 1,
                                Product = workUnit.Product.Name,
                                Material = workUnit.Material.Name,
                                Color = workUnit.Color.Name,
                                CurrentWorkArea = client,
                            });
                        }
                    }
                    else
                    {
                        newWorkArea.WorkUnits.Add(new WorkUnitReport
                        {
                            Quantity = 1,
                            Product = workUnit.Product.Name,
                            Material = workUnit.Material.Name,
                            Color = workUnit.Color.Name,
                            CurrentWorkArea = client,
                        });
                    }
                }

                deliveryOrderReport.Clients.Add(newWorkArea);
            }

            try
            {
                var rs = new ReportingService("http://192.168.1.99:5488", "Mirno", "MirnoReports");
                var jsonString = JsonConvert.SerializeObject(deliveryOrderReport);
                var report = rs.RenderByNameAsync("DeliveryOrder-main", jsonString).Result;

                Directory.CreateDirectory($"\\\\PC-FABRICA\\SistemaMirno\\TransferOrders");

                var filename = $"\\\\PC-FABRICA\\SistemaMirno\\TransferOrders\\TransferOrder{DateTime.Now.Ticks}.pdf";
                var stream = new FileStream(filename, FileMode.Create);

                report.Content.CopyTo(stream);
                stream.Close();

                ProcessStartInfo info = new ProcessStartInfo();
                info.Verb = "open";
                info.FileName = filename;

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
