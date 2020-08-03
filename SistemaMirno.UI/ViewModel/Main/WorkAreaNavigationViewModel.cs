using Prism.Events;
using SistemaMirno.Model;
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
    public class WorkAreaNavigationViewModel : ViewModelBase, IWorkAreaNavigationViewModel
    {
        private IWorkAreaRepository _productionAreaRepository;
        private IEventAggregator _eventAggregator;
        private WorkAreaWrapper _selectedProductionArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaNavigationViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaRepository">A <see cref="IWorkAreaRepository"/> representing the model repository.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> representing the event aggregator.</param>
        public WorkAreaNavigationViewModel(
            IWorkAreaRepository productionAreaRepository,
            IEventAggregator eventAggregator)
        {
            _productionAreaRepository = productionAreaRepository;
            ProductionAreas = new ObservableCollection<WorkAreaWrapper>();
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<WorkArea>>()
                .Subscribe(AfterProductionAreaSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<WorkArea>>()
                .Subscribe(AfterProductionAreaDeleted);
        }

        /// <summary>
        /// Gets or sets the selected production area.
        /// </summary>
        public WorkAreaWrapper SelectedProductionArea
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
                    _eventAggregator.GetEvent<ShowViewEvent<WorkArea>>()
                        .Publish(SelectedProductionArea.Id);
                }
            }
        }

        /// <summary>
        /// Gets the production areas stored in the view model.
        /// </summary>
        public ObservableCollection<WorkAreaWrapper> ProductionAreas { get; }

        /// <inheritdoc/>
        public async Task LoadAsync()
        {
            var productionAreas = await _productionAreaRepository.GetAllAsync();
            ProductionAreas.Clear();
            foreach (var area in productionAreas)
            {
                ProductionAreas.Add(new WorkAreaWrapper(area));
            }
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterProductionAreaSaved(AfterDataModelSavedEventArgs<WorkArea> args)
        {
            var item = ProductionAreas.SingleOrDefault(p => p.Id == args.Model.Id);

            if (item == null)
            {
                ProductionAreas.Add(new WorkAreaWrapper(args.Model));
            }
            else
            {
                item.Name = args.Model.Name;
            }
        }

        private void AfterProductionAreaDeleted(AfterDataModelDeletedEventArgs<WorkArea> args)
        {
            var item = ProductionAreas.SingleOrDefault(p => p.Id == args.Model.Id);

            if (item != null)
            {
                ProductionAreas.Remove(item);
            }
        }
    }
}