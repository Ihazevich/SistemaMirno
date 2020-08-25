using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Autofac.Features.Indexed;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Main
{
    /// <summary>
    /// Class representing the main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private bool? _dialogResult;
        private bool _navigationStatus = true;
        private IViewModelBase _navigationViewModel;
        private IViewModelBase _selectedViewModel;
        private bool _processing;

        // Visibility fields
        private Visibility _accountingVisibility;
        private Visibility _productionVisibility;
        private Visibility _productsVisibility;
        private Visibility _salesVisibility;
        private Visibility _humanResourcesVisibility;
        private Visibility _sysAdminVisibility;

        // Status bar fields
        private string _statusMessage;

        private IIndex<string, IViewModelBase> _viewModelCreator;

        private string _windowTitle =
            $"Sistema Mirno v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="viewModelCreator">The view model creator.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="dialogCoordinator">The dialog service.</param>
        public MainViewModel(
            IIndex<string, IViewModelBase> viewModelCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Principal", dialogCoordinator)
        {
            _viewModelCreator = viewModelCreator;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Subscribe(ChangeView);
            EventAggregator.GetEvent<NewWorkOrderEvent>()
                .Subscribe(NewMoveWorkOrder);
            EventAggregator.GetEvent<UserChangedEvent>()
                .Subscribe(UserChanged);
            EventAggregator.GetEvent<BranchChangedEvent>()
                .Subscribe(BranchChanged);
            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Subscribe(ChangeNavigationStatus);
            EventAggregator.GetEvent<ExitApplicationEvent>()
                .Subscribe(ExitApplication);
            EventAggregator.GetEvent<NotifyStatusBarEvent>()
                .Subscribe(UpdateStatusBar, ThreadOption.UIThread);
            EventAggregator.GetEvent<ShowDialogEvent>()
                .Subscribe(ShowDialog, ThreadOption.UIThread);

            EventAggregator.GetEvent<AskSessionInfoEvent>()
                .Subscribe(BroadcastSessionInfo);

            ChangeViewCommand = new DelegateCommand<string>(OnChangeViewExecute);
            CloseUserSessionCommand = new DelegateCommand(OnCloseUserSessionExecute);
            CloseApplicationCommand = new DelegateCommand(OnCloseApplicationExecute);

            ProductionVisibility = Visibility.Collapsed;
            SalesVisibility = Visibility.Collapsed;
            HumanResourcesVisibility = Visibility.Collapsed;
            AccountingVisibility = Visibility.Collapsed;
            SysAdminVisibility = Visibility.Collapsed;
            ProductsVisibility = Visibility.Collapsed;

            ShowLoginView();
        }

        private void BroadcastSessionInfo()
        {
            EventAggregator.GetEvent<BroadcastSessionInfoEvent>()
                .Publish(SessionInfo);
        }

        #endregion Constructors

        #region Properties

        public Visibility AccountingVisibility
        {
            get => _accountingVisibility;
            set
            {
                _accountingVisibility = value;
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

        public ICommand CloseUserSessionCommand { get; set; }
        
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

        public Visibility HumanResourcesVisibility
        {
            get => _humanResourcesVisibility;
            set
            {
                _humanResourcesVisibility = value;
                OnPropertyChanged();
            }
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

        public bool Processing
        {
            get => _processing;

            set
            {
                _processing = value;
                OnPropertyChanged();
            }
        }

        public Visibility ProductionVisibility
        {
            get => _productionVisibility;
            set
            {
                _productionVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility ProductsVisibility
        {
            get => _productsVisibility;
            set
            {
                _productsVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility SalesVisibility
        {
            get => _salesVisibility;
            set
            {
                _salesVisibility = value;
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

        public string StatusMessage
        {
            get => _statusMessage;

            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public Visibility SysAdminVisibility
        {
            get => _sysAdminVisibility;
            set
            {
                _sysAdminVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the main window title.
        /// </summary>
        public string WindowTitle
        {
            get { return _windowTitle; }

            set
            {
                _windowTitle = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public async void ShowDialog(ShowDialogEventArgs args)
        {
            await DialogCoordinator.ShowMessageAsync(this, args.Title, args.Message);
        }

        private async void BranchChanged(BranchChangedEventArgs args)
        {
            var sessionInfo = SessionInfo;
            sessionInfo.Branch = new BranchWrapper
            {
                Name = args.Name,
                Model =
                {
                    Id = args.BranchId,
                },
            };

            SessionInfo = sessionInfo;

            NavigationStatus = true;

            NavigationViewModel = _viewModelCreator[nameof(WorkAreaNavigationViewModel)];
            SelectedViewModel = null;

            UpdateStatusBar(new NotifyStatusBarEventArgs {Message = "Cargando navegación", Processing = true});
            await LoadAsync();
            ClearStatusBar();
        }

        private void ChangeNavigationStatus(bool arg)
        {
            if (NavigationStatus == false && arg)
            {
                SelectedViewModel = null;
            }

            NavigationStatus = arg;
        }

        private void ChangeView(ChangeViewEventArgs args)
        {
            NotifyStatusBar("Cambiando de vista", true);
            SelectedViewModel = _viewModelCreator[args.ViewModel];
            SelectedViewModel.LoadAsync();
            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(false);
            ClearStatusBar();
        }

        private void ExitApplication()
        {
            DialogResult = true;
        }

        private void NewMoveWorkOrder(NewWorkOrderEventArgs args)
        {
            /*
            ChangeView(new ChangeViewEventArgs { ViewModel = nameof(WorkOrderDetailViewModel), Id = args.DestinationWorkAreaId });
            ((WorkOrderDetailViewModel)SelectedViewModel).CreateNewWorkOrder(args.DestinationWorkAreaId, args.OriginWorkAreaId, args.WorkUnits);
            _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(false);*/
        }

        private void OnChangeViewExecute(string viewModel)
        {
            ChangeView(new ChangeViewEventArgs { ViewModel = viewModel, Id = -1 });
        }

        private void OnCloseApplicationExecute()
        {
            ExitApplication();
        }

        private void OnCloseUserSessionExecute()
        {
            var sessionInfo = SessionInfo;
            sessionInfo.UserLoggedIn = false;
            sessionInfo.User = null;
            SessionInfo = sessionInfo;

            SelectedViewModel = null;

            ShowLoginView();
        }

        private void ShowLoginView()
        {
            SelectedViewModel = _viewModelCreator[nameof(LoginViewModel)];
            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(false);
        }

        private void UpdateStatusBar(NotifyStatusBarEventArgs args)
        {
            StatusMessage = args.Message;
            Processing = args.Processing;
        }

        private void UserChanged(UserChangedEventArgs args)
        {
            var sessionInfo = SessionInfo;
            sessionInfo.UserLoggedIn = true;
            sessionInfo.User = new UserWrapper
            {
                Model =
                {
                    Username = args.Username,
                    EmployeeFullName = args.EmployeeFullName,
                    HasAccessToProduction = args.HasAccessToProduction,
                    HasAccessToLogistics = args.HasAccessToLogistics,
                    HasAccessToSales = args.HasAccessToSales,
                    HasAccessToAccounting = args.HasAccessToAccounting,
                    HasAccessToHumanResources = args.HasAccessToHumanResources,
                    IsSystemAdmin = args.IsSystemAdmin,
                },
            };

            SessionInfo = sessionInfo;

            AccountingVisibility = args.HasAccessToAccounting ? Visibility.Visible : Visibility.Collapsed;
            ProductionVisibility = args.HasAccessToProduction ? Visibility.Visible : Visibility.Collapsed;
            SalesVisibility = args.HasAccessToSales ? Visibility.Visible : Visibility.Collapsed;
            HumanResourcesVisibility = args.HasAccessToHumanResources ? Visibility.Visible : Visibility.Collapsed;
            SysAdminVisibility = args.IsSystemAdmin ? Visibility.Visible : Visibility.Collapsed;
            ProductsVisibility = args.HasAccessToSales || args.HasAccessToProduction
                ? Visibility.Visible
                : Visibility.Collapsed;

            ChangeView(new ChangeViewEventArgs
            {
                ViewModel = nameof(BranchSelectionViewModel),
            });
        }

        #endregion Methods
    }
}