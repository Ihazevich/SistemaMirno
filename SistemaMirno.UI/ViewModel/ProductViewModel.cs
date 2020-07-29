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

namespace SistemaMirno.UI.ViewModel
{
    public class ProductViewModel : ViewModelBase, IProductViewModel
    {
        private IProductDataService _productDataService;
        private IEventAggregator _eventAggregator;

        public ObservableCollection<Product> Products { get; set; }

        public ProductViewModel(IProductDataService productDataService, IEventAggregator eventAggregator)
        {
            Products = new ObservableCollection<Product>();
            _productDataService = productDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowProductViewEvent>()
                .Subscribe(ViewModelSelected);
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
