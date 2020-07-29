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
    public class ProductionAreaViewModel : ViewModelBase, IProductionAreaViewModel
    {
        private IProductionAreaDataService _productionAreaDataService;
        private IEventAggregator _eventAggregator;
        private ProductionArea _selectedArea;

        public ObservableCollection<ProductionArea> ProductionAreas { get; set; }
        public ProductionArea SelectedArea
        {
            get { return _selectedArea; }
            set
            {
                _selectedArea = value;
                OnPropertyChanged();
            }
        }

        public ProductionAreaViewModel(IProductionAreaDataService productionAreaDataService, IEventAggregator eventAggregator)
        {
            ProductionAreas = new ObservableCollection<ProductionArea>();
            _productionAreaDataService = productionAreaDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowProductionAreaViewEvent>()
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
            ProductionAreas.Clear();
            var areas = await _productionAreaDataService.GetAllAsync();
            foreach (var area in areas)
            {
                ProductionAreas.Add(area);
            }
        }
        protected override bool OnSaveCanExecute()
        {
            return true;
        }

        protected override void OnSaveExecute()
        {
            _productionAreaDataService.SaveAsync(SelectedArea);
            _eventAggregator.GetEvent<ReloadViewEvent>()
                .Publish("Navigation");
        }
    }
}
