using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
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
        private IMessageDialogService _messageDialogService;
        private string _windowTitle = $"Sistema Mirno v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="productionAreasViewModel">A <see cref="IProductionAreasNavigationViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="materialViewModel">A <see cref="IMaterialViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="colorViewModel">A <see cref="IColorViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="workUnitViewModel">A <see cref="IWorkUnitViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="productViewModel">A <see cref="IProductViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="productionAreaViewModel">A <see cref="IProductionAreaViewModel"/> instance representing the area navigation view model.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the area navigation view model.</param>
        public MainViewModel(
            IProductionAreasNavigationViewModel productionAreasViewModel,
            IMaterialViewModel materialViewModel,
            IColorViewModel colorViewModel,
            IWorkUnitViewModel workUnitViewModel,
            IProductViewModel productViewModel,
            IProductionAreaViewModel productionAreaViewModel,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            ProductionAreasNavigationViewModel = productionAreasViewModel;
            WorkUnitViewModel = workUnitViewModel;
            MaterialViewModel = materialViewModel;
            ColorViewModel = colorViewModel;
            ProductViewModel = productViewModel;
            ProductionAreaViewModel = productionAreaViewModel;

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
        public IProductionAreasNavigationViewModel ProductionAreasNavigationViewModel { get; }

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
        public IProductionAreaViewModel ProductionAreaViewModel { get; }

        /// <summary>
        /// Gets or sets the currently selected view model.
        /// </summary>
        public IViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
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
            await ProductionAreasNavigationViewModel.LoadAsync();
        }

        private void ChangeViewExecute(string viewModel)
        {
            switch(viewModel)
            {
                case "Materials":
                    _eventAggregator.GetEvent<ShowMaterialViewEvent>().
                        Publish();                    
                    break;
                case "Colors":
                    _eventAggregator.GetEvent<ShowColorViewEvent>().
                        Publish();
                    break;
                case "Products":
                    _eventAggregator.GetEvent<ShowProductViewEvent>().
                        Publish();
                    break;
                case "ProductionAreas":
                    _eventAggregator.GetEvent<ShowProductionAreaViewEvent>().
                        Publish();
                    break;
                case "Responsibles":
                    _eventAggregator.GetEvent<ShowResponsibleView>().
                        Publish();
                    break;
                case "Supervisors":
                    _eventAggregator.GetEvent<ShowSupervisorView>().
                        Publish();
                    break;
                default:
                    break;
            }
        }

        private bool ChangeViewCanExecute(string viewModel)
        {
            switch (viewModel)
            {
                case "Material":
                    if(nameof(SelectedViewModel).Equals(nameof(MaterialViewModel)))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
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
