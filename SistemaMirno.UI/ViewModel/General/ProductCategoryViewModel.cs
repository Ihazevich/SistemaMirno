using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ProductCategoryViewModel : ViewModelBase, IProductCategoryViewModel
    {
        private IProductCategoryRepository _productCategoryRepository;
        private IMessageDialogService _messageDialogService;
        private IEventAggregator _eventAggregator;
        private ProductCategoryWrapper _selectedProductCategory;
        private IProductCategoryDetailViewModel _productCategoryDetailViewModel;
        private Func<IProductCategoryDetailViewModel> _productCategoryDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public ProductCategoryViewModel(
            Func<IProductCategoryDetailViewModel> productCategoryDetailViewModelCreator,
            IProductCategoryRepository productCategoryRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _productCategoryDetailViewModelCreator = productCategoryDetailViewModelCreator;
            _productCategoryRepository = productCategoryRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowViewEvent<ProductCategoryViewModel>>()
                .Subscribe(ViewModelSelected);
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<ProductCategory>>()
                .Subscribe(AfterProductCategorySaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<ProductCategory>>()
                .Subscribe(AfterProductCategoryDeleted);

            ProductCategories = new ObservableCollection<ProductCategoryWrapper>();
            CreateNewProductCategoryCommand = new DelegateCommand(OnCreateNewProductCategoryExecute);
        }

        /// <summary>
        /// Gets the ProductCategory detail view model.
        /// </summary>
        public IProductCategoryDetailViewModel ProductCategoryDetailViewModel
        {
            get
            {
                return _productCategoryDetailViewModel;
            }

            private set
            {
                _productCategoryDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of ProductCategories.
        /// </summary>
        public ObservableCollection<ProductCategoryWrapper> ProductCategories { get; set; }

        /// <summary>
        /// Gets or sets the selected ProductCategory.
        /// </summary>
        public ProductCategoryWrapper SelectedProductCategory
        {
            get
            {
                return _selectedProductCategory;
            }

            set
            {
                OnPropertyChanged();
                _selectedProductCategory = value;
                if (_selectedProductCategory != null)
                {
                    UpdateDetailViewModel(_selectedProductCategory.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewProductCategory command.
        /// </summary>
        public ICommand CreateNewProductCategoryCommand { get; }

        /// <summary>
        /// Loads the view model and publishes the Change View event.
        /// </summary>
        public async void ViewModelSelected(int id)
        {
            await LoadAsync(id);
            _eventAggregator.GetEvent<ChangeViewEvent>().
                Publish(nameof(ProductCategoryViewModel));
        }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public async Task LoadAsync(int id)
        {
            ProductCategories.Clear();
            var productCategories = await _productCategoryRepository.GetAllAsync();
            foreach (var productCategory in productCategories)
            {
                ProductCategories.Add(new ProductCategoryWrapper(productCategory));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (ProductCategoryDetailViewModel != null && ProductCategoryDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            ProductCategoryDetailViewModel = _productCategoryDetailViewModelCreator();
            await ProductCategoryDetailViewModel.LoadAsync(id);
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterProductCategorySaved(AfterDataModelSavedEventArgs<ProductCategory> args)
        {
            var item = ProductCategories.SingleOrDefault(c => c.Id == args.Model.Id);

            if (item == null)
            {
                ProductCategories.Add(new ProductCategoryWrapper(args.Model));
                ProductCategoryDetailViewModel = null;
            }
            else
            {
                item.Name = args.Model.Name;
            }
        }

        private void AfterProductCategoryDeleted(AfterDataModelDeletedEventArgs<ProductCategory> args)
        {
            var item = ProductCategories.SingleOrDefault(c => c.Id == args.Model.Id);

            if (item != null)
            {
                ProductCategories.Remove(item);
            }

            ProductCategoryDetailViewModel = null;
        }

        private void OnCreateNewProductCategoryExecute()
        {
            UpdateDetailViewModel(null);
        }
    }
}