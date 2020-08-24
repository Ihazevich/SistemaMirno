using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public interface IDetailViewModelBase : IViewModelBase
    {
        /// <summary>
        /// Gets a value indicating whether the model has changes.
        /// </summary>
        bool HasChanges { get; }

        Task LoadDetailAsync(int id);

        void CreateNew();
    }
}