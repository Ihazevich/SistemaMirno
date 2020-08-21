using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class ProductDetailViewModel : DetailViewModelBase, IProductDetailViewModel
    {
        private ProductWrapper _product;
        private IProductCategoryRepository _productCategoryRepository;
        private IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDetailViewModel"/> class.
        /// </summary>
        /// <param name="productRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public ProductDetailViewModel(
            IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator, "Detalles de Producto")
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;

            _eventAggregator.GetEvent<AfterDataModelSavedEvent<ProductCategory>>()
                .Subscribe(AfterProductCategorySaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<ProductCategory>>()
                .Subscribe(AfterProductCategoryDeleted);

            ProductCategories = new ObservableCollection<ProductCategoryWrapper>();
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public ProductWrapper Product
        {
            get
            {
                return _product;
            }

            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProductCategoryWrapper> ProductCategories { get; }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? productId)
        {
            var product = productId.HasValue
                ? await _productRepository.GetByIdAsync(productId.Value)
                : CreateNewProduct();

            Product = new ProductWrapper(product);
            Product.PropertyChanged += Product_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (product.Id == 0)
            {
                // This triggers the validation.
                Product.Name = string.Empty;
            }

            await LoadProductCategoriesAsync();
        }

        protected override async void OnDeleteExecute()
        {
            _productRepository.Remove(Product.Model);
            await _productRepository.SaveAsync();
            RaiseDataModelDeletedEvent(Product.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Product != null && !Product.HasErrors && HasChanges;
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _productRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(Product.Model);
        }

        private void AfterProductCategoryDeleted(AfterDataModelDeletedEventArgs<ProductCategory> args)
        {
            var item = ProductCategories.SingleOrDefault(c => c.Id == args.Model.Id);

            if (item != null)
            {
                ProductCategories.Remove(item);
            }
        }

        private void AfterProductCategorySaved(AfterDataModelSavedEventArgs<ProductCategory> args)
        {
            var item = ProductCategories.SingleOrDefault(p => p.Id == args.Model.Id);

            if (item == null)
            {
                ProductCategories.Add(new ProductCategoryWrapper(args.Model));
            }
            else
            {
                item.Name = args.Model.Name;
            }
        }
        private Product CreateNewProduct()
        {
            var product = new Product();
            _productRepository.Add(product);
            return product;
        }

        private async Task LoadProductCategoriesAsync()
        {
            var categories = await _productCategoryRepository.GetAllAsync();
            ProductCategories.Clear();
            foreach (var category in categories)
            {
                ProductCategories.Add(new ProductCategoryWrapper(category));
            }
        }
        private void Product_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
            if (!HasChanges)
            {
                HasChanges = _productRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Product.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }
    }
}