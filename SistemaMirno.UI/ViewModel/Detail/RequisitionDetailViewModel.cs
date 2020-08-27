using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class RequisitionDetailViewModel : DetailViewModelBase, IRequisitionDetailViewModel
    {
        private IRequisitionRepository _requisitionRepository;
        private RequisitionWrapper _requisition;
        private WorkUnitWrapper _newWorkUnit;
        private bool _hasTargetDate;
        private Visibility _newWorkUnitGridVisibility;
        private Visibility _existingWorkUnitGridVisibility;
        private string _quantity;
        private bool _isForStock;

        public RequisitionDetailViewModel(
            IRequisitionRepository requisitionRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Pedido", dialogCoordinator)
        {
            _requisitionRepository = requisitionRepository;

            Clients = new ObservableCollection<ClientWrapper>();
            Products = new ObservableCollection<ProductWrapper>();
            Materials = new ObservableCollection<MaterialWrapper>();
            Colors = new ObservableCollection<ColorWrapper>();

            AddWorkUnitCommand = new DelegateCommand<object>(OnAddWorkUnitExecute);
            AddNewWorkUnitCommand = new DelegateCommand<object>(OnAddNewWorkUnitExecute, OnAddNewWorkUnitCanExecute);
        }

        private bool OnAddNewWorkUnitCanExecute(object arg)
        {
            return int.TryParse(Quantity, out _) && !NewWorkUnit.HasErrors;
        }

        private async void OnAddNewWorkUnitExecute(object obj)
        {
            var firstWorkAreaId = await _requisitionRepository.GetFirstWorkAreaIdAsync();

            if (!firstWorkAreaId.HasValue)
            {
                return;
            }

            var quantity = int.Parse(Quantity);

            for (var i = 0; i < quantity; i++)
            {
                var workUnit = new WorkUnit
                {
                    ProductId = NewWorkUnit.ProductId,
                    MaterialId = NewWorkUnit.MaterialId,
                    ColorId = NewWorkUnit.ColorId,
                    Delivered = false,
                    CreationDate = DateTime.Now,
                    TotalWorkTime = 0,
                    Details = NewWorkUnit.Details,
                };

                Requisition.Model.WorkUnits.Add(workUnit);
                WorkUnits.Add(workUnit);
            }
        }

        public bool ClientsEnabled
        {
            get => !IsForStock;
        }

        public bool IsForStock
        {
            get => _isForStock;

            set
            {
                _isForStock = value;
                Requisition.IsForStock = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ClientsEnabled));
            }
        }

        public string Quantity
        {
            get => _quantity;

            set
            {
                _quantity = int.TryParse(value, out _) ? value : string.Empty;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<WorkUnit> WorkUnits { get; set; }

        public ObservableCollection<ClientWrapper> Clients { get; }

        public ObservableCollection<ProductWrapper> Products { get; }

        public ObservableCollection<MaterialWrapper> Materials { get; }

        public ObservableCollection<ColorWrapper> Colors { get; }

        private void OnAddWorkUnitExecute(object obj)
        {
            switch (obj.ToString())
            {
                case "New":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        NewWorkUnitGridVisibility = Visibility.Visible;
                        ExistingWorkUnitGridVisibility = Visibility.Collapsed;
                    });
                    break;
                case "Existing":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        NewWorkUnitGridVisibility = Visibility.Collapsed;
                        ExistingWorkUnitGridVisibility = Visibility.Visible;
                    });
                    break;
            }
        }

        public Visibility NewWorkUnitGridVisibility
        {
            get => _newWorkUnitGridVisibility;

            set
            {
                _newWorkUnitGridVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility ExistingWorkUnitGridVisibility
        {
            get => _existingWorkUnitGridVisibility;

            set
            {
                _existingWorkUnitGridVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility NewButtonsVisibility => IsNew ? Visibility.Visible : Visibility.Collapsed;

        public Visibility DetailButtonsVisibility => IsNew ? Visibility.Collapsed : Visibility.Visible;

        public override bool IsNew
        {
            get => base.IsNew;

            set
            {
                base.IsNew = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NewButtonsVisibility));
                OnPropertyChanged(nameof(DetailButtonsVisibility));
            }
        }

        public RequisitionWrapper Requisition
        {
            get => _requisition;

            set
            {
                _requisition = value;
                OnPropertyChanged();
            }
        }

        public WorkUnitWrapper NewWorkUnit
        {
            get => _newWorkUnit;

            set
            {
                _newWorkUnit = value;
                OnPropertyChanged();
            }
        }

        public bool HasTargetDate
        {
            get => _hasTargetDate;

            set
            {
                _hasTargetDate = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddWorkUnitCommand { get; }
        public ICommand AddNewWorkUnitCommand { get; }
        public ICommand AddExistingWorkUnitCommand { get; }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _requisitionRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Requisition = new RequisitionWrapper(model);
                Requisition.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            if (IsNew)
            {
                await _requisitionRepository.AddAsync(Requisition.Model);
            }
            else
            {
                await _requisitionRepository.SaveAsync(Requisition.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Requisition);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            await _requisitionRepository.DeleteAsync(Requisition.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _requisitionRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Requisition.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            await LoadClients();
            await LoadMaterials();
            await LoadColors();
            await LoadProducts();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Requisition = new RequisitionWrapper();
                Requisition.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                Requisition.RequestedDate = DateTime.Now;
                Requisition.Fulfilled = false;

                NewWorkUnitGridVisibility = Visibility.Collapsed;
                ExistingWorkUnitGridVisibility = Visibility.Collapsed;

                IsNew = true;
                IsForStock = false;
                HasTargetDate = false;

                NewWorkUnit = new WorkUnitWrapper();
                NewWorkUnit.ProductId = 0;
                NewWorkUnit.MaterialId = 0;
                NewWorkUnit.ColorId = 0;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadProducts()
        {
            var products = await _requisitionRepository.GetAllProductsAsync();

            foreach (var product in products)
            {
                Application.Current.Dispatcher.Invoke(() => Products.Add(new ProductWrapper(product)));
            }
        }

        private async Task LoadColors()
        {
            var colors = await _requisitionRepository.GetAllColorsAsync();

            foreach (var color in colors)
            {
                Application.Current.Dispatcher.Invoke(() => Colors.Add(new ColorWrapper(color)));
            }
        }

        private async Task LoadMaterials()
        {
            var materials = await _requisitionRepository.GetAllMaterialsAsync();

            foreach (var material in materials)
            {
                Application.Current.Dispatcher.Invoke(() => Materials.Add(new MaterialWrapper(material)));
            }
        }

        private async Task LoadClients()
        {
            var clients = await _requisitionRepository.GetAllClientsAsync();

            foreach (var client in clients)
            {
                Application.Current.Dispatcher.Invoke(() => Clients.Add(new ClientWrapper(client)));
            }
        }
    }
}
