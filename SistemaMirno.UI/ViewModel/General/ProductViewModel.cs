using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FileHelpers;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.FileHelpers;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ProductViewModel : ViewModelBase, IProductViewModel
    {
        private IProductRepository _productRepository;
        private ProductWrapper _selectedProduct;

        public ProductViewModel(
            IProductRepository productRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Productos", dialogCoordinator)
        {
            _productRepository = productRepository;

            Products = new ObservableCollection<ProductWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
            ImportFromFileCommand = new DelegateCommand(OnImportFromFileExecute);
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedProduct.Id,
                    ViewModel = nameof(ProductDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedProduct != null;
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ProductDetailViewModel),
                });
        }

        public ObservableCollection<ProductWrapper> Products { get; }

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
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public ICommand ImportFromFileCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Products.Clear();

            var products = await _productRepository.GetAllAsync();

            foreach (var product in products)
            {
                Application.Current.Dispatcher.Invoke(() => Products.Add(new ProductWrapper(product)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private async void OnImportFromFileExecute()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                DefaultExt = ".csv",
                Filter = "CSV Files(.csv)|*.csv",
            };

            // Show open file dialog box
            var result = dialog.ShowDialog();

            var productsAdded = 0;
            var productsDiscarded = 0;

            // Process open file dialog box results
            if (result != true)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Visible);

            // Open document
            string filename = dialog.FileName;

            // Create FileHelpers Engine
            var engine = new FileHelperEngine<ProductFileHelper>();

            // Collection of processed products
            var products = new List<Product>();

            try
            {
                // Read Use:
                var data = engine.ReadFile(filename);

                foreach (ProductFileHelper product in data)
                {
                    // Check if the product name already exists in the database, if it exists discard it
                    if (await _productRepository.CheckForDuplicatesAsync(product.Name))
                    {
                        productsDiscarded++;
                        continue;
                    }

                    // Look for a matching product category
                    var category = await _productRepository.GetProductCategoryByNameAsync(product.Category);

                    if (category == null)
                    {
                        // If the category doesn't exist, check if it hasnt been created yet during this process
                        var matchingProduct = products.Find(p => p.ProductCategory.Name == product.Category);

                        if (matchingProduct != null)
                        {
                            category = matchingProduct.ProductCategory;
                        }
                        else
                        {
                            category = new ProductCategory
                            {
                                Name = product.Category,
                            };
                        }
                    }

                    // Create a new model
                    var newProduct = new Product
                    {
                        Code = product.Code,
                        Name = product.Name,
                        ProductCategory = category,
                        ProductionValue = product.ProductionValue,
                        WholesalerPrice = product.WholesalerPrice,
                        RetailPrice = product.RetailPrice,
                        IsCustom = false,
                        SketchupFile = string.Empty,
                        TemplateFile = string.Empty,
                    };

                    products.Add(newProduct);
                    productsAdded++;
                }

                // After all products added to the context, save to the database
                await _productRepository.AddRangeAsync(products);

                Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Collapsed);

                // Show a messagebox with the results
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Title = "Proceso completado",
                        Message = $"Registros nuevos: {productsAdded} | Registros descartados: {productsDiscarded}",
                    });
                EventAggregator.GetEvent<ChangeViewEvent>()
                    .Publish(new ChangeViewEventArgs
                    {
                        ViewModel = nameof(ProductViewModel),
                    });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => ProgressVisibility = Visibility.Collapsed);
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Title = "Error",
                        Message = $"Error [{ex.Message}]. Contacte al Administrador de Sistema.",
                    });
                EventAggregator.GetEvent<ChangeViewEvent>()
                    .Publish(new ChangeViewEventArgs
                    {
                        ViewModel = nameof(ProductViewModel),
                    });
            }
        }
    }
}
