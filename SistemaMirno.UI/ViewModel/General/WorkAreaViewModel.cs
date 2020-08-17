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
        private IWorkAreaRepository _workAreaRepository;
        private IMessageDialogService _messageDialogService;
        private WorkAreaWrapper _selectedArea;
        private IWorkAreaDetailViewModel _workAreaDetailViewModel;
        private Func<IWorkAreaDetailViewModel> _workAreaDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaViewModel"/> class.
        /// </summary>
        /// <param name="workAreaDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public WorkAreaViewModel(
            Func<IWorkAreaDetailViewModel> workAreaDetailViewModelCreator,
            IWorkAreaRepository workAreaRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
            : base(eventAggregator)
        {
            _workAreaDetailViewModelCreator = workAreaDetailViewModelCreator;
            _workAreaRepository = workAreaRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<WorkArea>>()
                .Subscribe(AfterWorkAreaSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<WorkArea>>()
                .Subscribe(AfterWorkAreaDeleted);

            WorkAreas = new ObservableCollection<WorkAreaWrapper>();
            CreateNewWorkAreaCommand = new DelegateCommand(OnCreateNewProductionAreaExecute);
        }

        /// <summary>
        /// Gets the Production Area detail view model.
        /// </summary>
        public IWorkAreaDetailViewModel WorkAreaDetailViewModel
        {
            get
            {
                return _workAreaDetailViewModel;
            }

            private set
            {
                _workAreaDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Production Areas.
        /// </summary>
        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; set; }

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
        public ICommand CreateNewWorkAreaCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync(int? id)
        {
            WorkAreas.Clear();
            var areas = await _workAreaRepository.GetAllAsync();
            foreach (var area in areas)
            {
                WorkAreas.Add(new WorkAreaWrapper(area));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (WorkAreaDetailViewModel != null && WorkAreaDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            WorkAreaDetailViewModel = _workAreaDetailViewModelCreator();
            await WorkAreaDetailViewModel.LoadAsync(id);
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
                WorkAreaDetailViewModel = null;
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

            WorkAreaDetailViewModel = null;
        }

        private void OnCreateNewProductionAreaExecute()
        {
            UpdateDetailViewModel(null);
        }
    }
}