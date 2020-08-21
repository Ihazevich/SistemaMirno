using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class ColorDetailViewModel : DetailViewModelBase, IColorDetailViewModel
    {
        private IColorRepository _colorRepository;
        private ColorWrapper _color;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorDetailViewModel"/> class.
        /// </summary>
        /// <param name="colorRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public ColorDetailViewModel(
            IColorRepository colorRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator, "Detalles de Lustre/Color")
        {
            _colorRepository = colorRepository;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public ColorWrapper Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? colorId)
        {
            var color = colorId.HasValue
                ? await _colorRepository.GetByIdAsync(colorId.Value)
                : CreateNewColor();

            Color = new ColorWrapper(color);
            Color.PropertyChanged += Color_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (color.Id == 0)
            {
                // This triggers the validation.
                Color.Name = string.Empty;
            }
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _colorRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(Color.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Color != null && !Color.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _colorRepository.Remove(Color.Model);
            await _colorRepository.SaveAsync();
            RaiseDataModelDeletedEvent(Color.Model);
        }

        private void Color_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _colorRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Color.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private Color CreateNewColor()
        {
            var color = new Color();
            _colorRepository.Add(color);
            return color;
        }
    }
}