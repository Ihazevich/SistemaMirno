using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
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
        private WorkAreaWrapper _selectedWorkArea;

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
            WorkAreas = new ObservableCollection<WorkAreaWrapper>();
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<WorkArea>>()
                .Subscribe(AfterWorkAreaSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<WorkArea>>()
                .Subscribe(AfterWorkAreaDeleted);
        }

        /// <summary>
        /// Gets or sets the selected work area.
        /// </summary>
        public WorkAreaWrapper SelectedWorkArea
        {
            get
            {
                return _selectedWorkArea;
            }

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
                if (_selectedWorkArea != null)
                {
                    _eventAggregator.GetEvent<ShowViewEvent<WorkUnitViewModel>>()
                        .Publish(SelectedWorkArea.Id);
                }
            }
        }

        /// <summary>
        /// Gets the production areas stored in the view model.
        /// </summary>
        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; }

        /// <inheritdoc/>
        public async Task LoadAsync()
        {
            var productionAreas = await _productionAreaRepository.GetAllAsync();
            WorkAreas.Clear();
            foreach (var area in productionAreas)
            {
                WorkAreas.Add(new WorkAreaWrapper(area));
            }
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterWorkAreaSaved(AfterDataModelSavedEventArgs<WorkArea> args)
        {
            var item = WorkAreas.SingleOrDefault(p => p.Id == args.Model.Id);

            if (item == null)
            {
                WorkAreas.Add(new WorkAreaWrapper(args.Model));
            }
            else
            {
                item.Name = args.Model.Name;
            }
        }

        private void AfterWorkAreaDeleted(AfterDataModelDeletedEventArgs<WorkArea> args)
        {
            var item = WorkAreas.SingleOrDefault(p => p.Id == args.Model.Id);

            if (item != null)
            {
                WorkAreas.Remove(item);
            }
        }
    }
}