using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Event;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IViewModelBase _selectedViewModel;
        private IEventAggregator _eventAggregator;

        public IProductionAreasViewModel ProductionAreasViewModel { get; }

        public IWorkUnitViewModel WorkUnitViewModel { get; }

        public IViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged();
            } 
        }

        public MainViewModel(IProductionAreasViewModel productionAreasViewModel,
            IWorkUnitViewModel workUnitViewModel, IEventAggregator eventAggregator)
        {
            ProductionAreasViewModel = productionAreasViewModel;
            WorkUnitViewModel = workUnitViewModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Subscribe(OnViewChanged);
        }

        public async Task LoadAsync()
        {
            await ProductionAreasViewModel.LoadAsync();
        }

        private void OnViewChanged(IViewModelBase viewModel)
        {
            SelectedViewModel = viewModel;
        }
        
    }
}
