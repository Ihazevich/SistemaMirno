using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel
{
    public class ProductViewModel : ViewModelBase, IProductViewModel
    {
        private IProductDataService _productDataService;
        private IEventAggregator _eventAggregator;

        private Product _selectedProduct;
        
        public ObservableCollection<Product> Products { get; set; }

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public ProductViewModel(IProductDataService productDataService, IEventAggregator eventAggregator)
        {
            Products = new ObservableCollection<Product>();
            _productDataService = productDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowProductViewEvent>()
                .Subscribe(ViewModelSelected);
        }

        protected override bool OnSaveCanExecute()
        {
            return true;
        }

        protected override void OnSaveExecute()
        {
            _productDataService.SaveAsync(SelectedProduct);
        }

        public async void ViewModelSelected()
        {
            await LoadAsync();
            _eventAggregator.GetEvent<ChangeViewEvent>().
                Publish(this);
        }

        public async Task LoadAsync()
        {
            Products.Clear();
            var products = await _productDataService.GetAllAsync();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }
    }
}
