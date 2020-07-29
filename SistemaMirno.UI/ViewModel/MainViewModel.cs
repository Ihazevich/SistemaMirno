using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.ViewModel
{
    /// <summary>
    /// Class representing the main view model.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IViewModelBase _selectedViewModel;
        private IEventAggregator _eventAggregator;
        private string _windowTitle = $"Sistema Mirno v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}";

        public MainViewModel(
            IProductionAreasNavigationViewModel productionAreasViewModel,
            IMaterialViewModel materialViewModel,
            IColorViewModel colorViewModel,
            IWorkUnitViewModel workUnitViewModel,
            IProductViewModel productViewModel,
            IProductionAreaViewModel productionAreaViewModel,
            IEventAggregator eventAggregator)
        {
            ProductionAreasNavigationViewModel = productionAreasViewModel;
            WorkUnitViewModel = workUnitViewModel;
            MaterialViewModel = materialViewModel;
            ColorViewModel = colorViewModel;
            ProductViewModel = productViewModel;
            ProductionAreaViewModel = productionAreaViewModel;

            _eventAggregator = eventAggregator;
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

        public IProductionAreasNavigationViewModel ProductionAreasNavigationViewModel { get; }
        public IWorkUnitViewModel WorkUnitViewModel { get; }
        public IMaterialViewModel MaterialViewModel { get; }
        public IColorViewModel ColorViewModel { get; }
        public IProductViewModel ProductViewModel { get; }
        public IProductionAreaViewModel ProductionAreaViewModel { get; }
        public IViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }
        public ICommand ChangeViewCommand { get; set; }

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

        public async Task LoadAsync()
        {
            await ProductionAreasNavigationViewModel.LoadAsync();
        }

        private void OnViewChanged(IViewModelBase viewModel)
        {
            SelectedViewModel = viewModel;
        }
        
    }
}
