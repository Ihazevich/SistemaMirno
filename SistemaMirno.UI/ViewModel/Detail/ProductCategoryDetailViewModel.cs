using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class ProductCategoryDetailViewModel : DetailViewModelBase, IProductCategoryDetailViewModel
    {
        private ProductCategoryWrapper _productCategory;
        private IProductCategoryRepository _productCategoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryDetailViewModel"/> class.
        /// </summary>
        /// <param name="productCategoryRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public ProductCategoryDetailViewModel(
            IProductCategoryRepository productCategoryRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _productCategoryRepository = productCategoryRepository;
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

        /// <inheritdoc/>
        public override async Task LoadAsync(int? productCategoryId)
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

        protected override async void OnDeleteExecute()
        {
            _productCategoryRepository.Remove(ProductCategory.Model);
            await _productCategoryRepository.SaveAsync();
            RaiseDataModelDeletedEvent(ProductCategory.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return ProductCategory != null && !ProductCategory.HasErrors && HasChanges;
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _productCategoryRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(ProductCategory.Model);
        }
        private ProductCategory CreateNewProductCategory()
        {
            var productCategory = new ProductCategory();
            _productCategoryRepository.Add(productCategory);
            return productCategory;
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
    }
}