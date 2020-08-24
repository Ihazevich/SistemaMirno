using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public interface IDetailViewModelBase : IViewModelBase
    {
        /// <summary>
        /// Gets a value indicating whether the model has changes.
        /// </summary>
        bool HasChanges { get; }

        bool IsNew { get; set; }

        Task LoadDetailAsync(int id);
    }
}