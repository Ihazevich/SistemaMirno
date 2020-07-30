using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class MaterialDetailViewModel : DetailViewModelBase, IMaterialDetailViewModel
    {
        private IMaterialRepository _materialRepository;
        private IEventAggregator _eventAggregator;
        private MaterialWrapper _material;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDetailViewModel"/> class.
        /// </summary>
        /// <param name="materialRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public MaterialDetailViewModel(
            IMaterialRepository materialRepository,
            IEventAggregator eventAggregator)
        {
            _materialRepository = materialRepository;
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public MaterialWrapper Material
        {
            get
            {
                return _material;
            }

            set
            {
                _material = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the database context has changes.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return _hasChanges;
            }

            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        /// <inheritdoc/>
        public async Task LoadAsync(int? materialId)
        {
            var material = materialId.HasValue
                ? await _materialRepository.GetByIdAsync(materialId.Value)
                : CreateNewProductionArea();

            Material = new MaterialWrapper(material);
            Material.PropertyChanged += ProductionArea_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (material.Id == 0)
            {
                // This triggers the validation.
                Material.Name = string.Empty;
            }
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _materialRepository.SaveAsync();
            //HasChanges = _productionAreaRepository.HasChanges();
            HasChanges = false;
            _eventAggregator.GetEvent<AfterMaterialSavedEvent>()
                .Publish(new AfterMaterialSavedEventArgs { Material = Material.Model });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Material != null && !Material.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _materialRepository.Remove(Material.Model);
            await _materialRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterMaterialDeletedEvent>()
                .Publish(Material.Id);
        }

        private void ProductionArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
            if (!HasChanges)
            {
                HasChanges = _materialRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Material.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private Material CreateNewProductionArea()
        {
            var material = new Material();
            _materialRepository.Add(material);
            return material;
        }
    }
}
