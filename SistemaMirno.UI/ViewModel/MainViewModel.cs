using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public IProductionAreasViewModel ProductionAreasViewModel { get; }

        public IWorkUnitViewModel WorkUnitViewModel { get; }

        public IViewModelBase SelectedViewModel { get; }

        public MainViewModel(IProductionAreasViewModel productionAreasViewModel,
            IWorkUnitViewModel workUnitViewModel)
        {
            ProductionAreasViewModel = productionAreasViewModel;
            WorkUnitViewModel = workUnitViewModel;
        }

        public async Task LoadAsync()
        {
            await ProductionAreasViewModel.LoadAsync();
        }
        
    }
}
