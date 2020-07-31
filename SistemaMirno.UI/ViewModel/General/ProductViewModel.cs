using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ProductViewModel : ViewModelBase, IProductViewModel
    {
        private IProductRepository _productRepository;
        private IMessageDialogService _messageDialogService;
        private IEventAggregator _eventAggregator;
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
        {
            _productDetailViewModelCreator = productDetailViewModelCreator;
            _productRepository = productRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowProductViewEvent>()
                .Subscribe(ViewModelSelected);
            _eventAggregator.GetEvent<AfterProductSavedEvent>()
                .Subscribe(AfterProductSaved);
            _eventAggregator.GetEvent<AfterProductDeletedEvent>()
                .Subscribe(AfterProductDeleted);

            Products = new ObservableCollection<ProductWrapper>();
            CreateNewProductCommand = new DelegateCommand(OnCreateNewProductExecute);
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

        /// <summary>
        /// Loads the view model and publishes the Change View event.
        /// </summary>
        public async void ViewModelSelected()
        {
            await LoadAsync();
            _eventAggregator.GetEvent<ChangeViewEvent>().
                Publish(this);
        }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public async Task LoadAsync()
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
        private void AfterProductSaved(AfterProductSavedEventArgs args)
        {
            var item = Products.SingleOrDefault(p => p.Id == args.Product.Id);

            if (item == null)
            {
                Products.Add(new ProductWrapper(args.Product));
                ProductDetailViewModel = null;
            }
            else
            {
                item.Name = args.Product.Name;
            }
        }

        private void AfterProductDeleted(int productId)
        {
            var item = Products.SingleOrDefault(p => p.Id == productId);

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
    }
}
