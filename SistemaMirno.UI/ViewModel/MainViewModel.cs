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
            IWorkUnitViewModel workUnitViewModel, IEventAggregator eventAggregator)
        {
            ProductionAreasViewModel = productionAreasViewModel;
            WorkUnitViewModel = workUnitViewModel;
            MaterialViewModel = materialViewModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Subscribe(OnViewChanged);

            ChangeViewCommand = new DelegateCommand<string>(ChangeViewExecute, ChangeViewCanExecute);
        }

        private void ChangeViewExecute(string viewModel)
        {
            Console.Write("Command raised, parameter={0}", viewModel);
            switch(viewModel)
            {
                case "Material":
                    _eventAggregator.GetEvent<ShowMaterialsEvent>().
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
