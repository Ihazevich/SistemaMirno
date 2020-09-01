using System;
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
using SistemaMirno.UI.ViewModel.Detail;
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
        private Visibility _menuVisibility;
        
        // Status bar fields
        private string _statusMessage;

        private readonly IIndex<string, IViewModelBase> _viewModelCreator;

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
                .Subscribe(NewWorkOrder);
            EventAggregator.GetEvent<UserChangedEvent>()
                .Subscribe(UserChanged);
            EventAggregator.GetEvent<BranchChangedEvent>()
                .Subscribe(BranchChanged);
            EventAggregator.GetEvent<ReloadNavigationViewEvent>()
                .Subscribe(ReloadNavigationView);
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

            ChangeView(new ChangeViewEventArgs
            {
                Id = null,
                ViewModel = nameof(LoginViewModel),
            });
        }

        public double TransformScaleX => Application.Current.MainWindow.RenderSize.Width / 1920.0;
        public double TransformScaleY => Application.Current.MainWindow.RenderSize.Height / 1080.0;

        private void ReloadNavigationView()
        {
            NavigationViewModel = _viewModelCreator[nameof(Main.NavigationViewModel)];
            NavigationViewModel.LoadAsync(SessionInfo.Branch.Id);
        }

        private void BroadcastSessionInfo()
        {
            EventAggregator.GetEvent<BroadcastSessionInfoEvent>()
                .Publish(SessionInfo);
        }

        #endregion Constructors

        #region Properties

        public Visibility MenuVisibility
        {
            get => _menuVisibility;

            set
            {
                _menuVisibility = value;
                OnPropertyChanged();
            }
        }

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

        public override async Task LoadAsync(int? id = null)
        {
        }

        public async void ShowDialog(ShowDialogEventArgs args)
        {
            var dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

            await DialogCoordinator.ShowMessageAsync(this, args.Title, args.Message,MessageDialogStyle.Affirmative,new MetroDialogSettings{CustomResourceDictionary = dictionary});
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
            SelectedViewModel = null;

            UpdateStatusBar(new NotifyStatusBarEventArgs {Message = "Cargando navegación", Processing = true});
            ReloadNavigationView();
            MenuVisibility = Visibility.Visible;
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
            if (args.ViewModel == null)
            {
                SelectedViewModel = null;
                return;
            }

            NotifyStatusBar("Cambiando de vista", true);
            if (args.ViewModel == nameof(LoginViewModel) || args.ViewModel == nameof(BranchSelectionViewModel))
            {
                MenuVisibility = Visibility.Collapsed;
            }

            SelectedViewModel = _viewModelCreator[args.ViewModel];
            SelectedViewModel.LoadAsync(args.Id);
            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(false);
            ClearStatusBar();
        }

        private void ExitApplication()
        {
            DialogResult = true;
        }

        private void NewWorkOrder(NewWorkOrderEventArgs args)
        {
            NotifyStatusBar("Cambiando de vista", true);
            SelectedViewModel = _viewModelCreator[nameof(WorkOrderDetailViewModel)];
            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(false);

            ((WorkOrderDetailViewModel)SelectedViewModel).CreateNewWorkOrder(args.DestinationWorkAreaId, args.OriginWorkAreaId, args.WorkUnits);
        }

        private void OnChangeViewExecute(string viewModel)
        {
            ChangeView(new ChangeViewEventArgs { ViewModel = viewModel, Id = null });
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

            ChangeView(new ChangeViewEventArgs
            {
                Id = null,
                ViewModel = nameof(LoginViewModel),
            });
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
            sessionInfo.User = args.User;

            SessionInfo = sessionInfo;

            AccountingVisibility = args.User.Model.HasAccessToAccounting ? Visibility.Visible : Visibility.Collapsed;
            ProductionVisibility = args.User.Model.HasAccessToProduction ? Visibility.Visible : Visibility.Collapsed;
            SalesVisibility = args.User.Model.HasAccessToSales ? Visibility.Visible : Visibility.Collapsed;
            HumanResourcesVisibility = args.User.Model.HasAccessToHumanResources ? Visibility.Visible : Visibility.Collapsed;
            SysAdminVisibility = args.User.Model.IsSystemAdmin ? Visibility.Visible : Visibility.Collapsed;
            ProductsVisibility = args.User.Model.HasAccessToSales || args.User.Model.HasAccessToProduction
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