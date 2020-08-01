using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.General;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel.Main
{
    /// <summary>
    /// Class representing the main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IViewModelBase _selectedViewModel;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private string _windowTitle = $"Sistema Mirno v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="workAreaNavigationViewModel">A <see cref="IWorkAreaNavigationViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="materialViewModel">A <see cref="IMaterialViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="colorViewModel">A <see cref="IColorViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="workUnitViewModel">A <see cref="IWorkUnitViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="productViewModel">A <see cref="IProductViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="workAreaViewModel">A <see cref="IWorkAreaViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the area navigation view model.</param>
        public MainViewModel(
            IWorkAreaNavigationViewModel workAreaNavigationViewModel,
            IMaterialViewModel materialViewModel,
            IColorViewModel colorViewModel,
            IProductCategoryViewModel productCategoryViewModel,
            IWorkUnitViewModel workUnitViewModel,
            IProductViewModel productViewModel,
            IWorkAreaViewModel workAreaViewModel,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            WorkAreaNavigationViewModel = workAreaNavigationViewModel;
            WorkUnitViewModel = workUnitViewModel;
            MaterialViewModel = materialViewModel;
            ColorViewModel = colorViewModel;
            ProductViewModel = productViewModel;
            WorkAreaViewModel = workAreaViewModel;
            ProductCategoryViewModel = productCategoryViewModel;

            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Subscribe(OnViewChanged);

            ChangeViewCommand = new DelegateCommand<string>(ChangeViewExecute, ChangeViewCanExecute);
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
        public IWorkAreaNavigationViewModel WorkAreaNavigationViewModel { get; }

        /// <summary>
        /// Gets the work unit view model.
        /// </summary>
        public IWorkUnitViewModel WorkUnitViewModel { get; }

        /// <summary>
        /// Gets the material view model.
        /// </summary>
        public IMaterialViewModel MaterialViewModel { get; }

        /// <summary>
        /// Gets the color view model.
        /// </summary>
        public IColorViewModel ColorViewModel { get; }

        /// <summary>
        /// Gets the product view model.
        /// </summary>
        public IProductViewModel ProductViewModel { get; }

        /// <summary>
        /// Gets the production area view model.
        /// </summary>
        public IWorkAreaViewModel WorkAreaViewModel { get; }

        /// <summary>
        /// Gets the product category area view model.
        /// </summary>
        public IProductCategoryViewModel ProductCategoryViewModel { get; }

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

        public async Task LoadAsync()
        {
            await WorkAreaNavigationViewModel.LoadAsync();
        }

        private void ChangeViewExecute(string viewModel)
        {
            switch (viewModel)
            {
                case "Materials":
                    _eventAggregator.GetEvent<ShowViewEvent<MaterialViewModel>>().
                        Publish(-1);
                    break;

                case "Colors":
                    _eventAggregator.GetEvent<ShowViewEvent<ColorViewModel>>().
                        Publish(-1);
                    break;

                case "Products":
                    _eventAggregator.GetEvent<ShowViewEvent<ProductViewModel>>().
                        Publish(-1);
                    break;

                case "ProductCategories":
                    _eventAggregator.GetEvent<ShowViewEvent<ProductCategoryViewModel>>().
                        Publish(-1);
                    break;

                case "ProductionAreas":
                    _eventAggregator.GetEvent<ShowViewEvent<WorkAreaViewModel>>().
                        Publish(-1);
                    break;

                default:
                    break;
            }
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

        private void OnViewChanged(IViewModelBase viewModel)
        {
            SelectedViewModel = viewModel;
        }
    }
}