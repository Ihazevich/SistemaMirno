using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.General
{
    public interface IWorkOrderViewModel
    {
        Task LoadAsync(int productionAreaId);
    }
}