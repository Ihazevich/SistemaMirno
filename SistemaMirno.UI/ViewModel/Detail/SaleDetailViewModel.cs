using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class SaleDetailViewModel : DetailViewModelBase, ISaleDetailViewModel
    {
        private ISaleRepository _saleRepository;
        private SaleWrapper _sale;
        private WorkUnitWrapper _workUnit;
        private WorkUnitWrapper _selectedWorkUnit;
        private RequisitionWrapper _requisition;
        private bool _isNewSale;

        private int _quantity;
        private int _requisitionWorkAreaId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDetailViewModel"/> class.
        /// </summary>
        /// <param name="colorRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public SaleDetailViewModel(
            ISaleRepository saleRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _saleRepository = saleRepository;

            Colors = new ObservableCollection<ColorWrapper>();
            Materials = new ObservableCollection<MaterialWrapper>();
            Products = new ObservableCollection<ProductWrapper>();
            Clients = new ObservableCollection<ClientWrapper>();

            ProductsCollection = CollectionViewSource.GetDefaultView(Products);

            AddWorkUnitCommand = new DelegateCommand(OnAddWorkUnitExecute);
            RemoveWorkUnitCommand = new DelegateCommand(OnRemoveWorkUnitExecute);
        }

        private void OnRemoveWorkUnitExecute()
        {
            throw new NotImplementedException();
        }

        private void OnAddWorkUnitExecute()
        {
            // Create a new Wrapper for the WorkUnit Model.
            var newWorkUnitWrapper = new WorkUnitWrapper(WorkUnit.Model);

            // Create an ammount of Work Units equal to the specified quantity.
            for (int i = 0; i < Quantity; i++)
            {
                var newWorkUnit = new WorkUnit
                {
                    WorkAreaId = _requisitionWorkAreaId,
                    ColorId = WorkUnit.ColorId,
                    MaterialId = WorkUnit.MaterialId,
                    ProductId = WorkUnit.ProductId,
                };

                // Set the model for the Work Unit, add extra details to it.
                WorkUnit.Model = newWorkUnit;
                WorkUnit.Product = Products.Where(p => p.Id == WorkUnit.ProductId).Single().Model;
                WorkUnit.Material = Materials.Where(m => m.Id == WorkUnit.MaterialId).Single().Model;
                WorkUnit.Color = Colors.Where(c => c.Id == WorkUnit.ColorId).Single().Model;

                // Add the Work Unit to the Observable Collection to display it on the view datagrid.
                WorkUnits.Add(WorkUnit);
            }

            // After processing, reset the WorkUnit and the quantity.
            WorkUnit = new WorkUnitWrapper(new WorkUnit());
            Quantity = 0;
        }

        public ICommand AddWorkUnitCommand { get; set; }

        public ICommand RemoveWorkUnitCommand { get; set; }

        public int Quantity
        {
            get => _quantity;

            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        
        public ICollectionView ProductsCollection { get; set; }

        public ObservableCollection<ProductWrapper> Products { get; set; }

        public ObservableCollection<ColorWrapper> Colors { get; set; }

        public ObservableCollection<MaterialWrapper> Materials { get; set; }

        public ObservableCollection<ClientWrapper> Clients { get; set; }

        public ObservableCollection<WorkUnitWrapper> WorkUnits { get; set; }

        public bool IsNewSale
        {
            get => _isNewSale;

            set
            {
                _isNewSale = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public SaleWrapper Sale
        {
            get
            {
                return _sale;
            }

            set
            {
                _sale = value;
                OnPropertyChanged();
            }
        }

        public WorkUnitWrapper WorkUnit
        {
            get
            {
                return _workUnit;
            }

            set
            {
                _workUnit = value;
                OnPropertyChanged();
            }
        }

        public WorkUnitWrapper SelectedWorkUnit
        {
            get
            {
                return _selectedWorkUnit;
            }

            set
            {
                _selectedWorkUnit = value;
                OnPropertyChanged();
            }
        }

        public RequisitionWrapper Requisition
        {
            get
            {
                return _requisition;
            }

            set
            {
                _requisition = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? saleId)
        {
            NotifyStatusBar("Cargando vista", true);

            var sale = saleId.HasValue
                ? await _saleRepository.GetByIdAsync(saleId.Value)
                : CreateNewSale();

            Sale = new SaleWrapper(sale);
            Sale.PropertyChanged += Client_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (sale.Id == 0)
            {
                // This triggers the validation.
                // TODO: Trigger validation
            }

            // Get the Requisition Work Area id
            _requisitionWorkAreaId = await _saleRepository.GetRequisitionAreaIdAsync();

            await LoadColorsAsync();
            await LoadMaterialsAsync();
            await LoadProductsAsync();
            await LoadClientsAsync();
            ClearStatusBar();
        }

        private async Task LoadColorsAsync()
        {
            NotifyStatusBar("Cargando colores", true);
            var colors = await _saleRepository.GetColorsAsync();
            Colors.Clear();
            foreach (var color in colors)
            {
                Colors.Add(new ColorWrapper(color));
            }
            ClearStatusBar();
        }

        private async Task LoadMaterialsAsync()
        {
            NotifyStatusBar("Cargando materiales", true);
            var materials = await _saleRepository.GetMaterialsAsync();
            Materials.Clear();
            foreach (var material in materials)
            {
                Materials.Add(new MaterialWrapper(material));
            }
            ClearStatusBar();
        }

        private async Task LoadProductsAsync()
        {
            NotifyStatusBar("Cargando productos", true);
            var products = await _saleRepository.GetProductsAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(new ProductWrapper(product));
            }
            ClearStatusBar();
        }

        private async Task LoadClientsAsync()
        {
            NotifyStatusBar("Cargando clientes", true);
            var clients = await _saleRepository.GetClientsAsync();
            Clients.Clear();
            foreach (var client in clients)
            {
                Clients.Add(new ClientWrapper(client));
            }
            ClearStatusBar();
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _saleRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(Sale.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Sale != null && !Sale.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _saleRepository.Remove(Sale.Model);
            await _saleRepository.SaveAsync();
            RaiseDataModelDeletedEvent(Sale.Model);
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(SaleViewModel) });
        }

        private void Client_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _saleRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Sale.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private Sale CreateNewSale()
        {
            NotifyStatusBar("Creando modelo de venta", true);
            var sale = new Sale();
            _saleRepository.Add(sale);
            ClearStatusBar();
            return sale;
        }
    }
}
