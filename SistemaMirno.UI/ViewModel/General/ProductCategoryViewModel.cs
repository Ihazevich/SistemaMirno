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
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ProductCategoryViewModel : ViewModelBase, IProductCategoryViewModel
    {
        private IProductCategoryRepository _productCategoryRepository;
        private Func<IProductCategoryRepository> _materialRepositoryCreator;
        private ProductCategoryWrapper _selectecProductCategory;

        public ProductCategoryViewModel(
            Func<IProductCategoryRepository> productCategoryRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Categorias de Productos", dialogCoordinator)
        {
            _materialRepositoryCreator = productCategoryRepository;

            ProductCategories = new ObservableCollection<ProductCategoryWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedProductCategory.Id,
                    ViewModel = nameof(ProductCategoryDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedProductCategory != null;
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ProductCategoryDetailViewModel),
                });
        }

        public ObservableCollection<ProductCategoryWrapper> ProductCategories { get; set; }

        public ProductCategoryWrapper SelectedProductCategory
        {
            get
            {
                return _selectecProductCategory;
            }

            set
            {
                OnPropertyChanged();
                _selectecProductCategory = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            ProductCategories.Clear();
            _productCategoryRepository = _materialRepositoryCreator();

            var categories = await _productCategoryRepository.GetAllAsync();

            foreach (var category in categories)
            {
                Application.Current.Dispatcher.Invoke(() => ProductCategories.Add(new ProductCategoryWrapper(category)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }
    }
}
