﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.ViewModel
{
    /// <summary>
    /// Production areas navigation view model class.
    /// </summary>
    public class ProductionAreasNavigationViewModel : ViewModelBase, IProductionAreasNavigationViewModel
    {
        private IProductionAreaRepository _productionAreaRepository;
        private IEventAggregator _eventAggregator;
        private ProductionArea _selectedProductionArea;

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
            ProductionAreas = new ObservableCollection<ProductionArea>();
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ReloadViewEvent>()
                .Subscribe(Reload);
        }

        /// <summary>
        /// Gets or sets the selected production area.
        /// </summary>
        public ProductionArea SelectedProductionArea
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
        public ObservableCollection<ProductionArea> ProductionAreas { get; }

        /// <inheritdoc/>
        public async Task LoadAsync()
        {
            var productionAreas = await _productionAreaRepository.GetAllAsync();
            ProductionAreas.Clear();
            foreach (var area in productionAreas)
            {
                ProductionAreas.Add(area);
            }
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private async void Reload(string viewModel)
        {
            // TODO: Make this method generic
            if (viewModel == "Navigation")
            {
                await LoadAsync();
            }
        }
    }
}
