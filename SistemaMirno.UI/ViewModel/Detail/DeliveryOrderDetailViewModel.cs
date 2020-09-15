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
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Reports;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class DeliveryOrderDetailViewModel : DetailViewModelBase
    {
        private readonly IDeliveryOrderRepository _deliveryOrderRepository;
        private DeliveryOrderWrapper _deliveryOrder;
        private Sale _selectedSale;
        private WorkUnit _selectedSaleWorkUnit;
        private WorkUnit _selectedDeliveryWorkUnit;
        private Employee _selectedResponsible;
        private string _saleClientFilter;
        private readonly PropertyGroupDescription _clientFullName = new PropertyGroupDescription("Sale.Client.FullName");
        private readonly PropertyGroupDescription _description = new PropertyGroupDescription("Description");

        private List<DeliveryUnit> _deliveryUnits;

        public DeliveryOrderDetailViewModel(
            IDeliveryOrderRepository deliveryOrderRepository,
            IEventAggregator eventAggregator, 
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalle de Orden de Entrega", dialogCoordinator)
        {
            _deliveryOrderRepository = deliveryOrderRepository;
            _deliveryUnits = new List<DeliveryUnit>();

            Sales = new ObservableCollection<Sale>();
            SaleWorkUnits = new ObservableCollection<WorkUnit>();
            DeliveryWorkUnits = new ObservableCollection<WorkUnit>();
            Vehicles = new ObservableCollection<Vehicle>();
            Responsibles = new ObservableCollection<Employee>();
            Deliveries = new ObservableCollection<Delivery>();

            SalesCollectionView = CollectionViewSource.GetDefaultView(Sales);
            SaleWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(SaleWorkUnits);
            DeliveryWorkUnitsCollectionView = CollectionViewSource.GetDefaultView(DeliveryWorkUnits);
            DeliveriesCollectionView = CollectionViewSource.GetDefaultView(Deliveries);

            SaleWorkUnitsCollectionView.GroupDescriptions.Add(_clientFullName);
            SaleWorkUnitsCollectionView.GroupDescriptions.Add(_description);
            DeliveryWorkUnitsCollectionView.GroupDescriptions.Add(_clientFullName);
            DeliveryWorkUnitsCollectionView.GroupDescriptions.Add(_description);

            AddWorkUnitCommand = new DelegateCommand(AddWorkUnitExecute, AddWorkUnitCanExecute);
            RemoveWorkUnitCommand = new DelegateCommand(RemoveWorkUnitExecute, RemoveWorkUnitCanExecute);
        }

        public void FilterSales()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SalesCollectionView.Filter = item =>
                {
                    return item is Sale vitem && vitem.Client.FullName.ToLowerInvariant()
                        .Contains(_saleClientFilter.ToLowerInvariant());
                };
            });
        }

        public string SaleClientFilter
        {
            get => _saleClientFilter;

            set
            {
                _saleClientFilter = value;
                OnPropertyChanged();
                FilterSales();
            }
        }

        public void FilterSaleWorkUnits()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SaleWorkUnitsCollectionView.Filter = item =>
                {
                    if (SelectedSale == null)
                    {
                        return false;
                    }

                    return item is WorkUnit vitem &&
                           vitem.SaleId == SelectedSale.Id;
                };
            });
        }

        public Sale SelectedSale
        {
            get => _selectedSale;

            set
            {
                _selectedSale = value;
                OnPropertyChanged();
                FilterSaleWorkUnits();
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

        private bool RemoveWorkUnitCanExecute()
        {
            return SelectedDeliveryWorkUnit != null;
        }

        private void RemoveWorkUnitExecute()
        {
            while (SelectedDeliveryWorkUnit != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (DeliveryWorkUnits.Where(s => s.SaleId == SelectedDeliveryWorkUnit.SaleId.Value).ToList()
                        .Count == 1)
                    {
                        Deliveries.Remove(Deliveries.Single(d => d.SaleId == SelectedDeliveryWorkUnit.SaleId.Value));
                    }

                    SaleWorkUnits.Add(SelectedDeliveryWorkUnit);
                    DeliveryWorkUnits.Remove(SelectedDeliveryWorkUnit);
                });
            }

            ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
        }

        private bool AddWorkUnitCanExecute()
        {
            return SelectedSaleWorkUnit != null;
        }

        private void AddWorkUnitExecute()
        {
            while (SelectedSaleWorkUnit != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (Deliveries.All(d => d.Sale.ClientId != SelectedSaleWorkUnit.Sale.ClientId))
                    {
                        var delivery = new Delivery
                        {
                            SaleId = SelectedSaleWorkUnit.SaleId.Value,
                            Sale = SelectedSaleWorkUnit.Sale,
                        };

                        Deliveries.Add(delivery);
                    }

                    DeliveryWorkUnits.Add(SelectedSaleWorkUnit);
                    SaleWorkUnits.Remove(SelectedSaleWorkUnit);
                });
            }

            ((DelegateCommand)AddWorkUnitCommand).RaiseCanExecuteChanged();
        }

        public WorkUnit SelectedSaleWorkUnit
        {
            get => _selectedSaleWorkUnit;

            set
            {
                _selectedSaleWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)AddWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkUnit SelectedDeliveryWorkUnit
        {
            get => _selectedDeliveryWorkUnit;

            set
            {
                _selectedDeliveryWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddWorkUnitCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

        public ICollectionView SalesCollectionView { get; }

        public ICollectionView SaleWorkUnitsCollectionView { get; }

        public ICollectionView DeliveryWorkUnitsCollectionView { get; }

        public ICollectionView DeliveriesCollectionView { get; }

        public ObservableCollection<Sale> Sales { get; }

        public ObservableCollection<WorkUnit> SaleWorkUnits { get; }

        public ObservableCollection<WorkUnit> DeliveryWorkUnits { get; }

        public ObservableCollection<Delivery> Deliveries { get; }

        public ObservableCollection<Vehicle> Vehicles { get; }

        public ObservableCollection<Employee> Responsibles { get; }

        public DeliveryOrderWrapper DeliveryOrder
        {
            get => _deliveryOrder;

            set
            {
                _deliveryOrder = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _deliveryOrderRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                DeliveryOrder = new DeliveryOrderWrapper(model);
                DeliveryOrder.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            foreach (var workUnit in DeliveryWorkUnits)
            {
                var deliveryUnit = new DeliveryUnit
                {
                    Delivered = false,
                    WorkUnitId = workUnit.Id,
                    WorkUnit = workUnit,
                };

                workUnit.Moving = true;

                _deliveryUnits.Add(deliveryUnit);
            }

            foreach (var delivery in Deliveries)
            {
                foreach (var deliveryUnit in _deliveryUnits.Where(d => d.WorkUnit.SaleId.Value == delivery.SaleId).ToList())
                {
                    delivery.DeliveryUnits.Add(deliveryUnit);
                }

                DeliveryOrder.Model.Deliveries.Add(delivery);
            }

            if (IsNew)
            {
                await _deliveryOrderRepository.AddAsync(DeliveryOrder.Model);
            }
            else
            {
                await _deliveryOrderRepository.SaveAsync(DeliveryOrder.Model);
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
                    ViewModel = nameof(DeliveryViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(DeliveryOrder);
        }

        protected override void OnCancelExecute()
        {
            base.OnCancelExecute();
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(DeliveryViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _deliveryOrderRepository.HasChanges();
            }

            if (e.PropertyName == nameof(DeliveryOrder.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadResponsiblesAsync();
            await LoadVehiclesAsync();

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            await LoadSalesAsync();
            await LoadWorkUnitsAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                DeliveryOrder = new DeliveryOrderWrapper();
                DeliveryOrder.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                DeliveryOrder.Date = DateTime.Today;
                DeliveryOrder.ResponsibleId = 0;
                DeliveryOrder.VehicleId = 0;
                DeliveryOrder.KmBefore = 0;
                DeliveryOrder.KmAfter = 0;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadVehiclesAsync()
        {
            var vehicles = await _deliveryOrderRepository.GetAllVehiclesAsync();

            foreach (var vehicle in vehicles)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Vehicles.Add(vehicle);
                });
            }
        }

        private async Task LoadResponsiblesAsync()
        {
            var responsibles = await _deliveryOrderRepository.GetAllLogisticResponsiblesAsync();

            foreach (var responsible in responsibles)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Responsibles.Add(responsible);
                });
            }
        }

        private async Task LoadSalesAsync()
        {
            var sales = await _deliveryOrderRepository.GetAllNonDeliveredSalesAsync();

            foreach (var sale in sales)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Sales.Add(sale);
                });
            }
        }

        private async Task LoadWorkUnitsAsync()
        {
            foreach (var sale in Sales)
            {
                foreach (var workUnit in sale.WorkUnits.Where(w => w.Delivered == false))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SaleWorkUnits.Add(workUnit);
                    });
                }
            }
        }

        private async Task CreateDeliveryOrderReport()
        {
            // Create a new report class with the Work Order data.
            var deliveryOrderReport = new DeliveryOrderReport
            {
                Date = DateTime.Today.Date.ToString("d"),
                Responsible = SelectedResponsible.FullName,
            };

            foreach (var delivery in Deliveries)
            {
                var client = new ClientReport
                {
                    Name = delivery.Sale.Client.FullName,
                    Address = delivery.Sale.Client.Address,
                    City = delivery.Sale.Client.City,
                    PhoneNumber = delivery.Sale.Client.PhoneNumber,
                };

                // Select all work units in the current delivery
                var workUnits = DeliveryWorkUnits.Where(w => w.Sale.Client.FullName == delivery.Sale.Client.FullName).ToList();

                // Create the reports for each Work Unit in the Work Area
                foreach (var workUnit in workUnits)
                {
                    // If there is already a work unit in the client report, check to group the similar ones
                    // else just add the Work Unit.
                    if (client.WorkUnits.Count > 0)
                    {
                        var found = false;
                        foreach (var workUnitReport in client.WorkUnits)
                        {
                            // If there is a work unit in the report that has the same properties, just add to the quantity.
                            if (workUnitReport.Product == workUnit.Product.Name
                                && workUnitReport.Material == workUnit.Material.Name
                                && workUnitReport.Color == workUnit.Color.Name
                                && workUnitReport.CurrentWorkArea == workUnit.CurrentWorkArea.Name)
                            {
                                workUnitReport.Quantity++;
                                found = true;
                                break;
                            }
                        }

                        // If there wasn't any work unit with the same properties, add the work unit to the report.
                        if (!found)
                        {
                            client.WorkUnits.Add(new WorkUnitReport
                            {
                                Quantity = 1,
                                Product = workUnit.Product.Name,
                                Material = workUnit.Material.Name,
                                Color = workUnit.Color.Name,
                                CurrentWorkArea = workUnit.CurrentWorkArea.Name,
                            });
                        }
                    }
                    else
                    {
                        client.WorkUnits.Add(new WorkUnitReport
                        {
                            Quantity = 1,
                            Product = workUnit.Product.Name,
                            Material = workUnit.Material.Name,
                            Color = workUnit.Color.Name,
                            CurrentWorkArea = workUnit.CurrentWorkArea.Name,
                        });
                    }
                }

                deliveryOrderReport.Clients.Add(client);
            }

            try
            {
                var rs = new ReportingService("http://192.168.1.99:5488", "Mirno", "MirnoReports");
                var jsonString = JsonConvert.SerializeObject(deliveryOrderReport);
                var report = rs.RenderByNameAsync("DeliveryOrder-main", jsonString).Result;

                Directory.CreateDirectory($"\\\\PC-FABRICA\\SistemaMirno\\DeliveryOrders");

                var filename = $"\\\\PC-FABRICA\\SistemaMirno\\DeliveryOrders\\DeliveryOrder{DateTime.Now.Ticks}.pdf";
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
