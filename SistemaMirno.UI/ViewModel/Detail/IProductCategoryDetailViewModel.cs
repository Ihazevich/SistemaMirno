using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public interface IProductCategoryDetailViewModel
    {
        /// <summary>
        /// Loads the view model with a data model that matches the provided id.
        /// </summary>
        /// <param name="productCategoryId">An <see cref="int"/> value representing the id.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task LoadAsync(int? productCategoryId);

        bool HasChanges { get; }
    }
}