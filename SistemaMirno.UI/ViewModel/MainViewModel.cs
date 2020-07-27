using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using System.Collections.ObjectModel;

namespace SistemaMirno.UI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private IDataService _areaDataService;
        private ProductionArea _selectedProductionArea;

        public ObservableCollection<ProductionArea> ProductionAreas { get; set; }
        public ProductionArea SelectedProductionArea
        {
            get { return _selectedProductionArea; }
            set
            {
                _selectedProductionArea = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(IDataService areaDataService)
        {
            ProductionAreas = new ObservableCollection<ProductionArea>();
            _areaDataService = areaDataService;
        }

        public void Load()
        {
            var areas = _areaDataService.GetAll();
            foreach (var area in areas)
            {
                ProductionAreas.Add(area as ProductionArea);
            }
        }
        
    }
}
