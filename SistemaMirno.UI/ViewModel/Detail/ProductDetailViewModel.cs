using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
    public class ProductDetailViewModel : DetailViewModelBase, IProductDetailViewModel
    {
        private IProductRepository _productRepository;
        private ProductWrapper _product;
        private ProductCategoryWrapper _newProductCategory;
        private bool _createNewProductCategory;

        public ProductDetailViewModel(
            IProductRepository productRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Producto", dialogCoordinator)
        {
            _productRepository = productRepository;
            ProductCategories = new ObservableCollection<ProductCategoryWrapper>();

            SelectFileCommand = new DelegateCommand<object>(OnSelectFileExecute);
        }
        
        public ProductWrapper Product
        {
            get => _product;

            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProductCategoryWrapper> ProductCategories { get; set; }

        public ICommand SelectFileCommand { get; }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _productRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Product = new ProductWrapper(model);
                Product.PropertyChanged += Model_PropertyChanged;
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
                await _productRepository.AddAsync(Product.Model);
            }
            else
            {
                await _productRepository.SaveAsync(Product.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ProductViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Product);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            await _productRepository.DeleteAsync(Product.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ProductViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ProductViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _productRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Product.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadProductCategories();

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                Product = new ProductWrapper();
                Product.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                
                Product.Code = string.Empty;
                Product.Name = string.Empty;
                Product.ProductCategoryId = 0;
                Product.ProductionValue = 0;
                Product.WholesalerPrice = 0;
                Product.RetailPrice = 0;
                Product.IsCustom = false;
                Product.SketchupFile = string.Empty;
                Product.TemplateFile = string.Empty;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadProductCategories()
        {
            var categories = await _productRepository.GetAllProductCategoriesAsync();

            foreach (var category in categories)
            {
                Application.Current.Dispatcher.Invoke(() => ProductCategories.Add(new ProductCategoryWrapper(category)));
            }
        }

        private void OnSelectFileExecute(object obj)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "Archivos PDF(.pdf)|*.pdf"; // Filter files by extension

            bool? result = dlg.ShowDialog();
            // Process open file dialog box results
            if (result == true)
            {
                if (obj.ToString() == "Sketchup")
                {
                    Application.Current.Dispatcher.Invoke(() => Product.SketchupFile = dlg.FileName);
                }
                else if (obj.ToString() == "Template")
                {
                    Application.Current.Dispatcher.Invoke(() => Product.TemplateFile = dlg.FileName);
                }
            }
        }
    }
}
