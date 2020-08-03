using SistemaMirno.UI.Wrapper;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.General
{
    public interface IAreaConnectionViewModel
    {
        Task LoadAsync(int productionAreaId);
    }
}