using System.ComponentModel;

namespace SistemaMirno.UI.ViewModel
{
    public interface IViewModelBase
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
}