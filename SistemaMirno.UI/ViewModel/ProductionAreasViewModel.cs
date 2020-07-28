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
    public class ProductionAreasViewModel : ViewModelBase, IProductionAreasViewModel
    {
        private IAreaDataService _areaDataService;
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
                    _eventAggregator.GetEvent<ShowProductionAreaWorkUnitsEvent>()
                        .Publish(_selectedProductionArea.Id);
                }
            }
        }
        public ObservableCollection<ProductionArea> ProductionAreas { get; }


        public ProductionAreasViewModel(IAreaDataService areaDataService,
            IEventAggregator eventAggregator)
        {
            _areaDataService = areaDataService;
            ProductionAreas = new ObservableCollection<ProductionArea>();
            _eventAggregator = eventAggregator;
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
    }
}
