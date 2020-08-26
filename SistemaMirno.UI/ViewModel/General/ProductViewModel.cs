using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ProductViewModel : ViewModelBase, IProductViewModel
    {
        private IProductRepository _productRepository;
        private Func<IProductRepository> _productRepositoryCreator;
        private ProductWrapper _selectedProduct;

        public ProductViewModel(
            Func<IProductRepository> productRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Productos", dialogCoordinator)
        {
            _productRepositoryCreator = productRepositoryCreator;

            Products = new ObservableCollection<ProductWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
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

        public ObservableCollection<ProductWrapper> Products { get; set; }

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

        public override async Task LoadAsync(int? id = null)
        {
            Products.Clear();
            _productRepository = _productRepositoryCreator();

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
    }
}
