using Prism.Events;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.Main
{
    /// <summary>
    /// Production areas navigation view model class.
    /// </summary>
    public class ProductionAreasNavigationViewModel : ViewModelBase, IProductionAreasNavigationViewModel
    {
        private IProductionAreaRepository _productionAreaRepository;
        private IEventAggregator _eventAggregator;
        private ProductionAreaWrapper _selectedProductionArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionAreasNavigationViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaRepository">A <see cref="IProductionAreaRepository"/> representing the model repository.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> representing the event aggregator.</param>
        public ProductionAreasNavigationViewModel(
            IProductionAreaRepository productionAreaRepository,
            IEventAggregator eventAggregator)
        {
            _productionAreaRepository = productionAreaRepository;
            ProductionAreas = new ObservableCollection<ProductionAreaWrapper>();
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterProductionAreaSavedEvent>()
                .Subscribe(AfterProductionAreaSaved);
            _eventAggregator.GetEvent<AfterProductionAreaDeletedEvent>()
                .Subscribe(AfterProductionAreaDeleted);
        }

        /// <summary>
        /// Gets or sets the selected production area.
        /// </summary>
        public ProductionAreaWrapper SelectedProductionArea
        {
            get
            {
                return _selectedProductionArea;
            }

            set
            {
                _selectedProductionArea = value;
                OnPropertyChanged();
                if (_selectedProductionArea != null)
                {
                    _eventAggregator.GetEvent<ShowWorkUnitsViewEvent>()
                        .Publish(_selectedProductionArea.Id);
                }
            }
        }

        /// <summary>
        /// Gets the production areas stored in the view model.
        /// </summary>
        public ObservableCollection<ProductionAreaWrapper> ProductionAreas { get; }

        /// <inheritdoc/>
        public async Task LoadAsync()
        {
            var productionAreas = await _productionAreaRepository.GetAllAsync();
            ProductionAreas.Clear();
            foreach (var area in productionAreas)
            {
                ProductionAreas.Add(new ProductionAreaWrapper(area));
            }
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterProductionAreaSaved(AfterProductionAreaSavedEventArgs args)
        {
            var item = ProductionAreas.SingleOrDefault(p => p.Id == args.ProductionArea.Id);

            if (item == null)
            {
                ProductionAreas.Add(new ProductionAreaWrapper(args.ProductionArea));
            }
            else
            {
                item.Name = args.ProductionArea.Name;
            }
        }

        private void AfterProductionAreaDeleted(int productionAreaId)
        {
            var item = ProductionAreas.SingleOrDefault(p => p.Id == productionAreaId);

            if (item != null)
            {
                ProductionAreas.Remove(item);
            }
        }
    }
}