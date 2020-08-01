using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class ProductDetailViewModel : DetailViewModelBase, IProductDetailViewModel
    {
        private IProductRepository _productRepository;
        private IProductCategoryRepository _productCategoryRepository;
        private IEventAggregator _eventAggregator;
        private ProductWrapper _product;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDetailViewModel"/> class.
        /// </summary>
        /// <param name="productRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public ProductDetailViewModel(
            IProductRepository productRepository,
            IProductCategoryRepository productCategoryRepository,
            IEventAggregator eventAggregator)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<AfterProductCategorySavedEvent>()
                .Subscribe(AfterProductCategorySaved);
            _eventAggregator.GetEvent<AfterProductCategoryDeletedEvent>()
                .Subscribe(AfterProductCategoryDeleted);

            ProductCategories = new ObservableCollection<ProductCategoryWrapper>();
        }

        private void AfterProductCategoryDeleted(int productCategoryId)
        {
            var item = ProductCategories.SingleOrDefault(c => c.Id == productCategoryId);

            if (item != null)
            {
                ProductCategories.Remove(item);
            }
        }

        private void AfterProductCategorySaved(AfterProductCategorySavedEventArgs args)
        {
            var item = ProductCategories.SingleOrDefault(p => p.Id == args.ProductCategory.Id);

            if (item == null)
            {
                ProductCategories.Add(new ProductCategoryWrapper(args.ProductCategory));
            }
            else
            {
                item.Name = args.ProductCategory.Name;
            }
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

        /// <summary>
        /// Gets or sets a value indicating whether the database context has changes.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return _hasChanges;
            }

            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        /// <inheritdoc/>
        public async Task LoadAsync(int? productId)
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

        private async Task LoadProductCategoriesAsync()
        {
            var categories = await _productCategoryRepository.GetAllAsync();
            ProductCategories.Clear();
            foreach (var category in categories)
            {
                ProductCategories.Add(new ProductCategoryWrapper(category));
            }
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _productRepository.SaveAsync();
            //HasChanges = _productionAreaRepository.HasChanges();
            HasChanges = false;
            _eventAggregator.GetEvent<AfterProductSavedEvent>()
                .Publish(new AfterProductSavedEventArgs { Product = Product.Model });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Product != null && !Product.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _productRepository.Remove(Product.Model);
            await _productRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterProductDeletedEvent>()
                .Publish(Product.Id);
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

        private Product CreateNewProduct()
        {
            var product = new Product();
            _productRepository.Add(product);
            return product;
        }
    }
}