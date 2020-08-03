using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    /// <summary>
    /// View Model for Production Areas.
    /// </summary>
    public class WorkAreaViewModel : ViewModelBase, IWorkAreaViewModel
    {
        private IWorkAreaRepository _productionAreaRepository;
        private IMessageDialogService _messageDialogService;
        private IEventAggregator _eventAggregator;
        private WorkAreaWrapper _selectedArea;
        private IWorkAreaDetailViewModel _productionAreaDetailViewModel;
        private Func<IWorkAreaDetailViewModel> _productionAreaDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public WorkAreaViewModel(
            Func<IWorkAreaDetailViewModel> productionAreaDetailViewModelCreator,
            IWorkAreaRepository productionAreaRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _productionAreaDetailViewModelCreator = productionAreaDetailViewModelCreator;
            _productionAreaRepository = productionAreaRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowViewEvent<WorkAreaViewModel>>()
                .Subscribe(ViewModelSelected);
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<WorkArea>>()
                .Subscribe(AfterProductionAreaSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<WorkArea>>()
                .Subscribe(AfterProductionAreaDeleted);

            ProductionAreas = new ObservableCollection<WorkAreaWrapper>();
            CreateNewProductionAreaCommand = new DelegateCommand(OnCreateNewProductionAreaExecute);
        }

        /// <summary>
        /// Gets the Production Area detail view model.
        /// </summary>
        public IWorkAreaDetailViewModel ProductionAreaDetailViewModel
        {
            get
            {
                return _productionAreaDetailViewModel;
            }

            private set
            {
                _productionAreaDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Production Areas.
        /// </summary>
        public ObservableCollection<WorkAreaWrapper> ProductionAreas { get; set; }

        /// <summary>
        /// Gets or sets the selected Production Area.
        /// </summary>
        public WorkAreaWrapper SelectedArea
        {
            get
            {
                return _selectedArea;
            }

            set
            {
                OnPropertyChanged();
                _selectedArea = value;
                if (_selectedArea != null)
                {
                    UpdateDetailViewModel(_selectedArea.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewProductionArea.
        /// </summary>
        public ICommand CreateNewProductionAreaCommand { get; }

        /// <summary>
        /// Loads the view model and publishes the Change View event.
        /// </summary>
        public async void ViewModelSelected(int id)
        {
            await LoadAsync(id);
            _eventAggregator.GetEvent<ChangeViewEvent>().
                Publish(nameof(WorkAreaViewModel));
        }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public async Task LoadAsync(int id)
        {
            ProductionAreas.Clear();
            var areas = await _productionAreaRepository.GetAllAsync();
            foreach (var area in areas)
            {
                ProductionAreas.Add(new WorkAreaWrapper(area));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (ProductionAreaDetailViewModel != null && ProductionAreaDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            ProductionAreaDetailViewModel = _productionAreaDetailViewModelCreator();
            await ProductionAreaDetailViewModel.LoadAsync(id);
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
                ProductionAreaDetailViewModel = null;
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

            ProductionAreaDetailViewModel = null;
        }

        private void OnCreateNewProductionAreaExecute()
        {
            UpdateDetailViewModel(null);
        }
    }
}