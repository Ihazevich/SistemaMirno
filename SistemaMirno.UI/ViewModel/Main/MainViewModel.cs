using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Autofac.Features.Indexed;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Main
{
    /// <summary>
    /// Class representing the main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IViewModelBase _selectedViewModel;
        private IViewModelBase _navigationViewModel;
        private IIndex<string, IViewModelBase> _viewModelCreator;
        private IMessageDialogService _messageDialogService;
        private string _windowTitle = $"Sistema Mirno v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
        private bool? _dialogResult;

        // User data fields.
        private bool _userLoggedIn = false;
        private string _username;
        private int _userAccessLevel;

        private bool _navigationStatus = true;

        // Status bar fields
        private string _statusMessage;
        private bool _processing;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="viewModelCreator">The view model creator.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="messageDialogService">The message dialog service.</param>
        public MainViewModel(
            IIndex<string, IViewModelBase> viewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
            : base (eventAggregator)
        {
            _viewModelCreator = viewModelCreator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Subscribe(ChangeView);
            _eventAggregator.GetEvent<NewWorkOrderEvent>()
                .Subscribe(NewMoveWorkOrder);
            _eventAggregator.GetEvent<UserChangedEvent>()
                .Subscribe(UserChanged);
            _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Subscribe(ChangeNavigationStatus);
            _eventAggregator.GetEvent<ExitApplicationEvent>()
                .Subscribe(ExitApplication);
            _eventAggregator.GetEvent<NotifyStatusBarEvent>()
                .Subscribe(UpdateStatusBar, ThreadOption.UIThread);

            ChangeViewCommand = new DelegateCommand<string>(OnChangeViewExecute, ChangeViewCanExecute);
            CloseApplicationCommand = new DelegateCommand(OnCloseApplicationExecute);

            ShowLoginView();
        }

        private void UpdateStatusBar(NotifyStatusBarEventArgs args)
        {
            StatusMessage = args.Message;
            Processing = args.Processing;
        }

        public string StatusMessage
        {
            get => _statusMessage;

            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public bool Processing
        {
            get => _processing;

            set
            {
                _processing = value;
                OnPropertyChanged();
            }
        }

        private void ChangeNavigationStatus(bool arg)
        {
            if (NavigationStatus == false && arg)
            {
                SelectedViewModel = null;
            }

            NavigationStatus = arg;
        }

        private void ExitApplication()
        {
            DialogResult = true;
        }

        public bool NavigationStatus
        {
            get => _navigationStatus;

            set
            {
                _navigationStatus = value;
                OnPropertyChanged();
            }
        }

        private void UserChanged(UserChangedEventArgs args)
        {
            UserLoggedIn = true;
            NavigationStatus = true;
            Username = args.Username;
            UserAccessLevel = args.AccessLevel;

            NavigationViewModel = _viewModelCreator[nameof(WorkAreaNavigationViewModel)];
            SelectedViewModel = null;

            LoadAsync(null);
        }

        /// <summary>
        /// Gets or sets if a user is currently logged in.
        /// </summary>
        public bool UserLoggedIn
        {
            get => _userLoggedIn;

            set
            {
                _userLoggedIn = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the username of the current user.
        /// </summary>
        public string Username
        {
            get => _username;

            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the access level of the current user.
        /// </summary>
        public int UserAccessLevel
        {
            get => _userAccessLevel;

            set
            {
                _userAccessLevel = value;
                OnPropertyChanged();
            }
        }

        private void OnCloseApplicationExecute()
        {
            ExitApplication();
        }

        /// <summary>
        /// Gets or sets the dialog result of the main view.
        /// </summary>
        public bool? DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the main window title.
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return _windowTitle;
            }

            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the area navigation view model.
        /// </summary>
        public IViewModelBase NavigationViewModel 
        {
            get => _navigationViewModel;

            set
            {
                _navigationViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the currently selected view model.
        /// </summary>
        public IViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;

            set
            {
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the change view command.
        /// </summary>
        public ICommand ChangeViewCommand { get; set; }

        /// <summary>
        /// Gets or sets the close application command.
        /// </summary>
        public ICommand CloseApplicationCommand { get; set; }

        public override async Task LoadAsync(int? id)
        {
            await NavigationViewModel.LoadAsync(-1);
        }

        private void OnChangeViewExecute(string viewModel)
        {
            ChangeView(new ChangeViewEventArgs { ViewModel = viewModel, Id = -1 });
        }

        private bool ChangeViewCanExecute(string viewModel = null)
        {
            switch (viewModel)
            {
                case "Colors":
                    return !nameof(SelectedViewModel).Equals(nameof(ColorViewModel));

                case "Materials":
                    return !nameof(SelectedViewModel).Equals(nameof(MaterialViewModel));

                case "ProductionAreas":
                    return !nameof(SelectedViewModel).Equals(nameof(WorkAreaViewModel));

                case "ProductCategories":
                    return !nameof(SelectedViewModel).Equals(nameof(ProductCategoryViewModel));

                default:
                    return true;
            }
        }

        private void ChangeView(ChangeViewEventArgs args)
        {
            SelectedViewModel = _viewModelCreator[args.ViewModel];
            SelectedViewModel.LoadAsync(args.Id);
            _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(false);
        }

        private void NewMoveWorkOrder(NewWorkOrderEventArgs args)
        {
            ChangeView(new ChangeViewEventArgs { ViewModel = nameof(WorkOrderDetailViewModel), Id = args.DestinationWorkAreaId });
            ((WorkOrderDetailViewModel)SelectedViewModel).CreateNewWorkOrder(args.DestinationWorkAreaId, args.OriginWorkAreaId, args.WorkUnits);
            _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(false);
        }

        private void ShowLoginView()
        {
            SelectedViewModel = _viewModelCreator[nameof(LoginViewModel)];
            _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(false);
        }
    }
}