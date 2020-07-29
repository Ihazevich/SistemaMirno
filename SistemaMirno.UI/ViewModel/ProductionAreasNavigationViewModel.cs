using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Event;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    public class ProductionAreasNavigationViewModel : ViewModelBase, IProductionAreasNavigationViewModel
    {
        private IProductionAreaDataService _areaDataService;
        private IEventAggregator _eventAggregator;

        private ProductionArea _selectedProductionArea;

        public ProductionArea SelectedProductionArea
        {
            get { return _selectedProductionArea; }
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
        public ObservableCollection<ProductionArea> ProductionAreas { get; }


        public ProductionAreasNavigationViewModel(IProductionAreaDataService areaDataService,
            IEventAggregator eventAggregator)
        {
            _areaDataService = areaDataService;
            ProductionAreas = new ObservableCollection<ProductionArea>();
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ReloadViewEvent>()
                .Subscribe(Reload);
        }

        public async Task LoadAsync()
        {
            var productionAreas = await _areaDataService.GetAllAsync();
            ProductionAreas.Clear();
            foreach (var area in productionAreas)
            {
                ProductionAreas.Add(area);
            }
        }

        private async void Reload(string viewModel)
        {
            if(viewModel == "Navigation")
            {
                await LoadAsync();
            }
        }
    }
}
