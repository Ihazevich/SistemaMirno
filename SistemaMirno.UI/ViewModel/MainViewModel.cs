using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Event;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IViewModelBase _selectedViewModel;
        private IEventAggregator _eventAggregator;

        public IProductionAreasViewModel ProductionAreasViewModel { get; }

        public IWorkUnitViewModel WorkUnitViewModel { get; }
        public IMaterialViewModel MaterialViewModel { get; }
        public IColorViewModel ColorViewModel { get; }
        public IProductViewModel ProductViewModel { get; }

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

        public MainViewModel(IProductionAreasViewModel productionAreasViewModel, IMaterialViewModel materialViewModel,
            IColorViewModel colorViewModel, IWorkUnitViewModel workUnitViewModel, IProductViewModel productViewModel,
            IEventAggregator eventAggregator)
        {
            ProductionAreasViewModel = productionAreasViewModel;
            WorkUnitViewModel = workUnitViewModel;
            MaterialViewModel = materialViewModel;
            ColorViewModel = colorViewModel;
            ProductViewModel = productViewModel;

            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Subscribe(OnViewChanged);

            ChangeViewCommand = new DelegateCommand<string>(ChangeViewExecute, ChangeViewCanExecute);
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
