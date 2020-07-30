using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    /// <summary>
    /// Class representing the detailed view model of a single Production Area.
    /// </summary>
    public class ProductionAreaDetailViewModel : DetailViewModelBase<ProductionAreaWrapper>, IProductionAreaDetailViewModel
    {
        private IProductionAreaRepository _productionAreaRepository;
        private IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionAreaDetailViewModel"/> class.
        /// </summary>
        /// <param name="model">A <see cref="ProductionAreaWrapper"/> instance to use as the data model.</param>
        /// <param name="productionAreaRepository">A <see cref="IProductionAreaRepository"/> instance to use as the data repository.</param>
        public ProductionAreaDetailViewModel(ProductionAreaWrapper model, IProductionAreaRepository productionAreaRepository, IEventAggregator eventAggregator)
            : base(model)
        {
            Model = model;
            _productionAreaRepository = productionAreaRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UpdateProductionAreaDetailView>()
                .Subscribe(UpdateViewModel);
        }

        /// <inheritdoc/>
        public async Task LoadAsync(int productionAreaId)
        {
            Model = new ProductionAreaWrapper(await _productionAreaRepository.GetByIdAsync(productionAreaId));
            Model.PropertyChanged += Model_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _productionAreaRepository.SaveAsync();
            _eventAggregator.GetEvent<ReloadViewEvent>()
                .Publish("Navigation");
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Model != null && !Model.HasErrors;
        }

        private async void UpdateViewModel(int id)
        {
            await LoadAsync(id);
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
            if (e.PropertyName == nameof(Model.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

    }
}
