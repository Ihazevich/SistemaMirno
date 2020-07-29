using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel
{
    /// <summary>
    /// View Model for Production Areas.
    /// </summary>
    public class ProductionAreaViewModel : ViewModelBase, IProductionAreaViewModel
    {
        private IProductionAreaRepository _productionAreaRepository;
        private IEventAggregator _eventAggregator;
        private ProductionAreaWrapper _selectedArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionAreaViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaRepository">A <see cref="IProductionAreaRepository"/> instance representing the data repository.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public ProductionAreaViewModel(
            IProductionAreaRepository productionAreaRepository,
            IEventAggregator eventAggregator)
        {
            ProductionAreas = new ObservableCollection<ProductionAreaWrapper>();
            _productionAreaRepository = productionAreaRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowProductionAreaViewEvent>()
                .Subscribe(ViewModelSelected);
        }

        /// <summary>
        /// Gets or sets the collection of Production Areas.
        /// </summary>
        public ObservableCollection<ProductionAreaWrapper> ProductionAreas { get; set; }

        /// <summary>
        /// Gets or sets the selected Production Area.
        /// </summary>
        public ProductionAreaWrapper SelectedArea
        {
            get
            {
                return _selectedArea;
            }

            set
            {
                _selectedArea = value;
                OnPropertyChanged();
                SelectedArea.PropertyChanged += SelectedArea_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Loads the view model and publishes the Change View event.
        /// </summary>
        public async void ViewModelSelected()
        {
            await LoadAsync();
            _eventAggregator.GetEvent<ChangeViewEvent>().
                Publish(this);
        }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public async Task LoadAsync()
        {
            ProductionAreas.Clear();
            var areas = await _productionAreaRepository.GetAllAsync();
            foreach (var area in areas)
            {
                ProductionAreas.Add(new ProductionAreaWrapper(area));
            }

        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return SelectedArea != null && !SelectedArea.HasErrors;
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _productionAreaRepository.SaveAsync();
            _eventAggregator.GetEvent<ReloadViewEvent>()
                .Publish("Navigation");
        }

        private void SelectedArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
            if (e.PropertyName == nameof(SelectedArea.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
