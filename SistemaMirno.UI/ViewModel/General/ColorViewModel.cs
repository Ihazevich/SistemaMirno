using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ColorViewModel : ViewModelBase, IColorViewModel
    {
        private IColorRepository _colorRepository;
        private IMessageDialogService _messageDialogService;
        private IEventAggregator _eventAggregator;
        private ColorWrapper _selectedColor;
        private IColorDetailViewModel _colorDetailViewModel;
        private Func<IColorDetailViewModel> _colorDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorViewModel"/> class.
        /// </summary>
        /// <param name="colorDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public ColorViewModel(
            Func<IColorDetailViewModel> colorDetailViewModelCreator,
            IColorRepository colorRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _colorDetailViewModelCreator = colorDetailViewModelCreator;
            _colorRepository = colorRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowViewEvent<ColorViewModel>>()
                .Subscribe(ViewModelSelected);
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<Color>>()
                .Subscribe(AfterColorSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<Color>>()
                .Subscribe(AfterColorDeleted);

            Colors = new ObservableCollection<ColorWrapper>();
            CreateNewColorCommand = new DelegateCommand(OnCreateNewColorExecute);
        }

        /// <summary>
        /// Gets the Color detail view model.
        /// </summary>
        public IColorDetailViewModel ColorDetailViewModel
        {
            get
            {
                return _colorDetailViewModel;
            }

            private set
            {
                _colorDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Colors.
        /// </summary>
        public ObservableCollection<ColorWrapper> Colors { get; set; }

        /// <summary>
        /// Gets or sets the selected Color.
        /// </summary>
        public ColorWrapper SelectedColor
        {
            get
            {
                return _selectedColor;
            }

            set
            {
                OnPropertyChanged();
                _selectedColor = value;
                if (_selectedColor != null)
                {
                    UpdateDetailViewModel(_selectedColor.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewColor command.
        /// </summary>
        public ICommand CreateNewColorCommand { get; }

        /// <summary>
        /// Loads the view model and publishes the Change View event.
        /// </summary>
        public async void ViewModelSelected(int id)
        {
            await LoadAsync();
            _eventAggregator.GetEvent<ChangeViewEvent>().
                Publish(this);
        }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public async Task LoadAsync()
        {
            Colors.Clear();
            var colors = await _colorRepository.GetAllAsync();
            foreach (var color in colors)
            {
                Colors.Add(new ColorWrapper(color));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (ColorDetailViewModel != null && ColorDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            ColorDetailViewModel = _colorDetailViewModelCreator();
            await ColorDetailViewModel.LoadAsync(id);
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterColorSaved(AfterDataModelSavedEventArgs<Color> args)
        {
            var item = Colors.SingleOrDefault(c => c.Id == args.Model.Id);

            if (item == null)
            {
                Colors.Add(new ColorWrapper(args.Model));
                ColorDetailViewModel = null;
            }
            else
            {
                item.Name = args.Model.Name;
            }
        }

        private void AfterColorDeleted(AfterDataModelDeletedEventArgs<Color> args)
        {
            var item = Colors.SingleOrDefault(m => m.Id == args.Model.Id);

            if (item != null)
            {
                Colors.Remove(item);
            }

            ColorDetailViewModel = null;
        }

        private void OnCreateNewColorExecute()
        {
            UpdateDetailViewModel(null);
        }
    }
}