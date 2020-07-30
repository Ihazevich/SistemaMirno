using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    /// <summary>
    /// Class representing the detailed view model of a single Production Area.
    /// </summary>
    public class ProductionAreaDetailViewModel : DetailViewModelBase, IProductionAreaDetailViewModel
    {
        private IProductionAreaRepository _productionAreaRepository;
        private IEventAggregator _eventAggregator;
        private ProductionAreaWrapper _productionArea;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionAreaDetailViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public ProductionAreaDetailViewModel(
            IProductionAreaRepository productionAreaRepository,
            IEventAggregator eventAggregator)
        {
            _productionAreaRepository = productionAreaRepository;
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public ProductionAreaWrapper ProductionArea
        {
            get
            {
                return _productionArea;
            }

            set
            {
                _productionArea = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the database context has changes.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return _hasChanges;
            }

            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        /// <inheritdoc/>
        public async Task LoadAsync(int productionAreaId)
        {
            var productionArea = await _productionAreaRepository.GetByIdAsync(productionAreaId);

            ProductionArea = new ProductionAreaWrapper(productionArea);
            ProductionArea.PropertyChanged += ProductionArea_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _productionAreaRepository.SaveAsync();
            HasChanges = _productionAreaRepository.HasChanges();
            _eventAggregator.GetEvent<AfterProductionAreaSavedEvent>()
                .Publish(new AfterProductionAreaSavedEventArgs { ProductionArea = ProductionArea.Model });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return ProductionArea != null && !ProductionArea.HasErrors && HasChanges;
        }


        private void ProductionArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
            if (!HasChanges)
            {
                HasChanges = _productionAreaRepository.HasChanges();
            }

            if (e.PropertyName == nameof(ProductionArea.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

    }
}
