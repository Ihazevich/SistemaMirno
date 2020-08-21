using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class MaterialDetailViewModel : DetailViewModelBase, IMaterialDetailViewModel
    {
        private MaterialWrapper _material;
        private IMaterialRepository _materialRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDetailViewModel"/> class.
        /// </summary>
        /// <param name="materialRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public MaterialDetailViewModel(
            IMaterialRepository materialRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator, "Detalles de Material")
        {
            _materialRepository = materialRepository;
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

        /// <inheritdoc/>
        public override async Task LoadAsync(int? materialId)
        {
            var material = materialId.HasValue
                ? await _materialRepository.GetByIdAsync(materialId.Value)
                : CreateNewMaterial();

            Material = new MaterialWrapper(material);
            Material.PropertyChanged += Material_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (material.Id == 0)
            {
                // This triggers the validation.
                Material.Name = string.Empty;
            }
        }

        protected override async void OnDeleteExecute()
        {
            _materialRepository.Remove(Material.Model);
            await _materialRepository.SaveAsync();
            RaiseDataModelDeletedEvent(Material.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Material != null && !Material.HasErrors && HasChanges;
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _materialRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(Material.Model);
        }
        private Material CreateNewMaterial()
        {
            var material = new Material();
            _materialRepository.Add(material);
            return material;
        }

        private void Material_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
    }
}