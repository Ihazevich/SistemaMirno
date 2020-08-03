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
        private IEventAggregator _eventAggregator;
        private AreaConnectionWrapper _selectedAreaConnection;
        private IAreaConnectionDetailViewModel _areaConnectionDetailViewModel;
        private Func<IAreaConnectionDetailViewModel> _areaConnectionDetailViewModelCreator;
        private string _areaName;
        private int _areaId;

        public AreaConnectionViewModel(
            Func<IAreaConnectionDetailViewModel> areaConnectionDetailViewModelCreator,
            IAreaConnectionRepository areaConnectionRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _areaConnectionDetailViewModelCreator = areaConnectionDetailViewModelCreator;
            _areaConnectionRepository = areaConnectionRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowViewEvent<AreaConnectionViewModel>>()
                .Subscribe(OnWorkAreaAreaConnectionSelected);
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<AreaConnection>>()
                .Subscribe(AfterAreaConnectionSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<AreaConnection>>()
                .Subscribe(AfterAreaConnectionDeleted);

            AreaConnections = new ObservableCollection<AreaConnectionWrapper>();
            CreateNewAreaConnectionCommand = new DelegateCommand(OnCreateNewAreaConnectionExecute);
        }

        /// <summary>
        /// Gets or sets the production area name for the view.
        /// </summary>
        public string AreaName
        {
            get
            {
                return _areaName;
            }

            set
            {
                _areaName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the Color detail view model.
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

        /// <summary>
        /// Loads the view model and publishes the Change View event.
        /// </summary>
        public async void ViewModelSelected(int id)
        {
            await LoadAsync(id);
            _eventAggregator.GetEvent<ChangeViewEvent>().
                Publish(this);
        }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public async Task LoadAsync(int workAreaId)
        {
            AreaConnections.Clear();
            _areaId = workAreaId;
            var connections = await _areaConnectionRepository.GetByAreaIdAsync(workAreaId);
            AreaName = await _areaConnectionRepository.GetWorkAreaNameAsync(workAreaId);
            foreach (var connection in connections)
            {
                AreaConnections.Add(new AreaConnectionWrapper(connection));
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
            await AreaConnectionDetailViewModel.LoadAsync(id);
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
                item.ToWorkAreaId = args.Model.ToWorkAreaId;
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
        }

        private async void OnWorkAreaAreaConnectionSelected(int productionAreaId)
        {
            await LoadAsync(productionAreaId);
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(this);
        }
    }
}
