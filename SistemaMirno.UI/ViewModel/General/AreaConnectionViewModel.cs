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
    public class AreaConnectionViewModel : ViewModelBase, IAreaConnectionViewModel
    {

        private IAreaConnectionRepository _areaConnectionRepository;
        private IMessageDialogService _messageDialogService;
        private AreaConnectionWrapper _selectedAreaConnection;
        private IAreaConnectionDetailViewModel _areaConnectionDetailViewModel;
        private Func<IAreaConnectionDetailViewModel> _areaConnectionDetailViewModelCreator;
        private WorkAreaWrapper _workArea;

        public AreaConnectionViewModel(
            Func<IAreaConnectionDetailViewModel> areaConnectionDetailViewModelCreator,
            IAreaConnectionRepository areaConnectionRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
            : base (eventAggregator, "Conexiones de Area")
        {
            _areaConnectionDetailViewModelCreator = areaConnectionDetailViewModelCreator;
            _areaConnectionRepository = areaConnectionRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<AreaConnection>>()
                .Subscribe(AfterAreaConnectionSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<AreaConnection>>()
                .Subscribe(AfterAreaConnectionDeleted);

            AreaConnections = new ObservableCollection<AreaConnectionWrapper>();

            CreateNewAreaConnectionCommand = new DelegateCommand(OnCreateNewAreaConnectionExecute);
            GoBackCommand = new DelegateCommand(OnGoBackExecute);
        }

        /// <summary>
        /// Gets or sets the Work Area.
        /// </summary>
        public WorkAreaWrapper WorkArea
        {
            get
            {
                return _workArea;
            }

            set
            {
                _workArea = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the Area Connection detail view model.
        /// </summary>
        public IAreaConnectionDetailViewModel AreaConnectionDetailViewModel
        {
            get
            {
                return _areaConnectionDetailViewModel;
            }

            private set
            {
                _areaConnectionDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of AreaConnections.
        /// </summary>
        public ObservableCollection<AreaConnectionWrapper> AreaConnections { get; set; }

        /// <summary>
        /// Gets or sets the selected Color.
        /// </summary>
        public AreaConnectionWrapper SelectedAreaConnection
        {
            get
            {
                return _selectedAreaConnection;
            }

            set
            {
                OnPropertyChanged();
                _selectedAreaConnection = value;
                if (_selectedAreaConnection != null)
                {
                    UpdateDetailViewModel(_selectedAreaConnection.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewColor command.
        /// </summary>
        public ICommand CreateNewAreaConnectionCommand { get; }

        public ICommand GoBackCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync(int? workAreaId)
        {
            // Check if we have a Work Area Id.
            if (workAreaId.HasValue)
            {
                // Clear the connections collection.
                AreaConnections.Clear();

                // Get the Work Area for the given Id.
                WorkArea = new WorkAreaWrapper(await _areaConnectionRepository.GetWorkAreaByIdAsync(workAreaId.Value));

                // Add all the Work Area Connections to the observable collection.
                foreach (var connection in WorkArea.AreaConnections)
                {
                    AreaConnections.Add(new AreaConnectionWrapper(connection));
                }
            }
            else
            {
                throw new Exception("Calling AreaConnection view without areaId");
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (AreaConnectionDetailViewModel != null && AreaConnectionDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            AreaConnectionDetailViewModel = _areaConnectionDetailViewModelCreator();
            AreaConnectionDetailViewModel.LoadAsync(id);
            AreaConnectionDetailViewModel.SetWorkAreaId(WorkArea.Id);
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterAreaConnectionSaved(AfterDataModelSavedEventArgs<AreaConnection> args)
        {
            var item = AreaConnections.SingleOrDefault(c => c.Id == args.Model.Id);

            if (item == null)
            {
                AreaConnections.Add(new AreaConnectionWrapper(args.Model));
                AreaConnectionDetailViewModel = null;
            }
            else
            {
                item.ConnectedWorkAreaId = args.Model.ConnectedWorkAreaId;
            }
        }

        private void AfterAreaConnectionDeleted(AfterDataModelDeletedEventArgs<AreaConnection> args)
        {
            var item = AreaConnections.SingleOrDefault(m => m.Id == args.Model.Id);

            if (item != null)
            {
                AreaConnections.Remove(item);
            }

            AreaConnectionDetailViewModel = null;
        }

        private void OnCreateNewAreaConnectionExecute()
        {
            UpdateDetailViewModel(null);
            AreaConnectionDetailViewModel.SetWorkAreaId(WorkArea.Id);
        }

        private void OnGoBackExecute()
        {
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(WorkAreaViewModel), Id = -1 });
        }
    }
}
