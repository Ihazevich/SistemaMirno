using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.Indexed;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General;

namespace SistemaMirno.UI.ViewModel.Main
{
    /// <summary>
    /// Class representing the main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IViewModelBase _selectedViewModel;
        private IEventAggregator _eventAggregator;
        private IIndex<string, IViewModelBase> _viewModelCreator;
        private IMessageDialogService _messageDialogService;
        private string _windowTitle = $"Sistema Mirno v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}";

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
        {
            _viewModelCreator = viewModelCreator;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Subscribe(ChangeView);
            _eventAggregator.GetEvent<NewMoveWorkOrderEvent>()
                .Subscribe(NewMoveWorkOrder);

            ChangeViewCommand = new DelegateCommand<string>(OnChangeViewExecute, ChangeViewCanExecute);
            WorkAreaNavigationViewModel = _viewModelCreator[nameof(WorkAreaNavigationViewModel)];
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
        public IViewModelBase WorkAreaNavigationViewModel { get; }

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

        public override async Task LoadAsync(int? id)
        {
            await WorkAreaNavigationViewModel.LoadAsync(-1);
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
        }

        private void NewMoveWorkOrder(NewMoveWorkOrderEventArgs args)
        {
            ChangeView(new ChangeViewEventArgs { ViewModel = nameof(WorkOrderDetailViewModel), Id = args.WorkAreaId });
            ((WorkOrderDetailViewModel)SelectedViewModel).CreateNewWorkOrder(args.WorkUnits);
        }
    }
}