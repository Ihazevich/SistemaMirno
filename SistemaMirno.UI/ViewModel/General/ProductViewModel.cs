using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FileHelpers;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.FileHelpers;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ProductViewModel : ViewModelBase, IProductViewModel
    {
        private IProductRepository _productRepository;
        private IMessageDialogService _messageDialogService;
        private ProductWrapper _selectedProduct;
        private IProductDetailViewModel _productDetailViewModel;
        private Func<IProductDetailViewModel> _productDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public ProductViewModel(
            Func<IProductDetailViewModel> productDetailViewModelCreator,
            IProductRepository productRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
            : base (eventAggregator)
        {
            _productDetailViewModelCreator = productDetailViewModelCreator;
            _productRepository = productRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<Product>>()
                .Subscribe(AfterProductSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<Product>>()
                .Subscribe(AfterProductDeleted);

            Products = new ObservableCollection<ProductWrapper>();
            CreateNewProductCommand = new DelegateCommand(OnCreateNewProductExecute);
            ImportFromFileCommand = new DelegateCommand(OnImportFromFileExecute);
        }

        /// <summary>
        /// Gets the Product detail view model.
        /// </summary>
        public IProductDetailViewModel ProductDetailViewModel
        {
            get
            {
                return _productDetailViewModel;
            }

            private set
            {
                _productDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Products.
        /// </summary>
        public ObservableCollection<ProductWrapper> Products { get; set; }

        /// <summary>
        /// Gets or sets the selected Product.
        /// </summary>
        public ProductWrapper SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }

            set
            {
                OnPropertyChanged();
                _selectedProduct = value;
                if (_selectedProduct != null)
                {
                    UpdateDetailViewModel(_selectedProduct.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewProduct command.
        /// </summary>
        public ICommand CreateNewProductCommand { get; }

        public ICommand ImportFromFileCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync(int? id)
        {
            Products.Clear();
            var products = await _productRepository.GetAllAsync();
            foreach (var product in products)
            {
                Products.Add(new ProductWrapper(product));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (ProductDetailViewModel != null && ProductDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            ProductDetailViewModel = _productDetailViewModelCreator();
            await ProductDetailViewModel.LoadAsync(id);
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterProductSaved(AfterDataModelSavedEventArgs<Product> args)
        {
            var item = Products.SingleOrDefault(p => p.Id == args.Model.Id);

            if (item == null)
            {
                Products.Add(new ProductWrapper(args.Model));
                ProductDetailViewModel = null;
            }
            else
            {
                item.Name = args.Model.Name;
            }
        }

        private void AfterProductDeleted(AfterDataModelDeletedEventArgs<Product> args)
        {
            var item = Products.SingleOrDefault(p => p.Id == args.Model.Id);

            if (item != null)
            {
                Products.Remove(item);
            }

            ProductDetailViewModel = null;
        }

        private void OnCreateNewProductExecute()
        {
            UpdateDetailViewModel(null);
        }

        private async void OnImportFromFileExecute()
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV Files(.csv)|*.csv"; // Filter files by extension

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            int productsAdded = 0;
            int productsDiscarded = 0;

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;

                // Create FileHelpers Engine
                var engine = new FileHelperEngine<ProductFileHelper>();

                try
                {
                    // Read Use:
                    var data = engine.ReadFile(filename);

                    foreach (ProductFileHelper product in data)
                    {
                        // Look for a matching product category to reference the id
                        var categoryId = await _productRepository.GetProductCategoryIdAsync(product.Category);

                        // If the category doesn't exists, discard the product
                        if (categoryId == -1)
                        {
                            productsDiscarded++;
                            continue;
                        }

                        // Check if the product name already exists in the database, if it exists discard it
                        if (await _productRepository.CheckForDuplicatesAsync(product.Name))
                        {
                            productsDiscarded++;
                            continue;
                        }

                        // Create a new model 
                        var newProduct = new Product
                        {
                            Code = product.Code,
                            Name = product.Name,
                            ProductCategoryId = categoryId,
                            Price = product.Price,
                            WholesalePrice = product.WholesalePrice,
                            ProductionPrice = product.ProductionPrice,
                        };

                        _productRepository.Add(newProduct);
                        productsAdded++;
                    }

                    // After all products added to the context, save the context
                    await _productRepository.SaveAsync();

                    // Show a messagebox with the results 
                    _messageDialogService.ShowOkDialog(string.Format("Registros nuevos: {0} | Registros descartados: {1}", productsAdded, productsDiscarded), "Proceso completado");
                    _eventAggregator.GetEvent<ChangeViewEvent>()
                        .Publish(new ChangeViewEventArgs { ViewModel = nameof(ProductViewModel) });
                }
                catch (Exception ex)
                {
                    _messageDialogService.ShowOkDialog(ex.Message, "Error al leer el archivo");
                }
            }
        }
    }
}