using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class ProductCategoryDetailViewModel : DetailViewModelBase, IProductCategoryDetailViewModel
    {
        private IProductCategoryRepository _productCategoryRepository;
        private IEventAggregator _eventAggregator;
        private ProductCategoryWrapper _productCategory;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryDetailViewModel"/> class.
        /// </summary>
        /// <param name="productCategoryRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public ProductCategoryDetailViewModel(
            IProductCategoryRepository productCategoryRepository,
            IEventAggregator eventAggregator)
        {
            _productCategoryRepository = productCategoryRepository;
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public ProductCategoryWrapper ProductCategory
        {
            get
            {
                return _productCategory;
            }

            set
            {
                _productCategory = value;
                OnPropertyChanged();
            }
        }

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
        public async Task LoadAsync(int? productCategoryId)
        {
            var productCategory = productCategoryId.HasValue
                ? await _productCategoryRepository.GetByIdAsync(productCategoryId.Value)
                : CreateNewProductCategory();

            ProductCategory = new ProductCategoryWrapper(productCategory);
            ProductCategory.PropertyChanged += ProductCategory_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (productCategory.Id == 0)
            {
                // This triggers the validation.
                ProductCategory.Name = string.Empty;
            }
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _productCategoryRepository.SaveAsync();
            //HasChanges = _productionAreaRepository.HasChanges();
            HasChanges = false;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<ProductCategory>>()
                .Publish(new AfterDataModelSavedEventArgs<ProductCategory> { Model = ProductCategory.Model });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return ProductCategory != null && !ProductCategory.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _productCategoryRepository.Remove(ProductCategory.Model);
            await _productCategoryRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<ProductCategory>>()
                .Publish(new AfterDataModelDeletedEventArgs<ProductCategory> { Model = ProductCategory.Model });
        }

        private void ProductCategory_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _productCategoryRepository.HasChanges();
            }

            if (e.PropertyName == nameof(ProductCategory.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private ProductCategory CreateNewProductCategory()
        {
            var productCategory = new ProductCategory();
            _productCategoryRepository.Add(productCategory);
            return productCategory;
        }
    }
}