using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.General
{
    public interface IWorkUnitViewModel
    {
        Task LoadAsync(int productionAreaId);
    }
}