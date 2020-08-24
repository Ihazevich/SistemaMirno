using System.ComponentModel;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    public interface IViewModelBase
    {
        Task LoadAsync();
    }
}