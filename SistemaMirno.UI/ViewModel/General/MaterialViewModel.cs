﻿using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel.General
{
    public class MaterialViewModel : ViewModelBase, IMaterialViewModel
    {
        private IMaterialRepository _materialRepository;
        private IMessageDialogService _messageDialogService;
        private IEventAggregator _eventAggregator;
        private MaterialWrapper _selectedMaterial;
        private IMaterialDetailViewModel _materialDetailViewModel;
        private Func<IMaterialDetailViewModel> _materialDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public MaterialViewModel(
            Func<IMaterialDetailViewModel> materialDetailViewModelCreator,
            IMaterialRepository materialRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _materialDetailViewModelCreator = materialDetailViewModelCreator;
            _materialRepository = materialRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowMaterialViewEvent>()
                .Subscribe(ViewModelSelected);
            _eventAggregator.GetEvent<AfterMaterialSavedEvent>()
                .Subscribe(AfterMaterialSaved);
            _eventAggregator.GetEvent<AfterMaterialDeletedEvent>()
                .Subscribe(AfterMaterialDeleted);

            Materials = new ObservableCollection<MaterialWrapper>();
            CreateNewMaterialCommand = new DelegateCommand(OnCreateNewMaterialExecute);
        }

        /// <summary>
        /// Gets the Material detail view model.
        /// </summary>
        public IMaterialDetailViewModel MaterialDetailViewModel
        {
            get
            {
                return _materialDetailViewModel;
            }

            private set
            {
                _materialDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Materials.
        /// </summary>
        public ObservableCollection<MaterialWrapper> Materials { get; set; }

        /// <summary>
        /// Gets or sets the selected Material.
        /// </summary>
        public MaterialWrapper SelectedMaterial
        {
            get
            {
                return _selectedMaterial;
            }

            set
            {
                OnPropertyChanged();
                _selectedMaterial = value;
                if (_selectedMaterial != null)
                {
                    UpdateDetailViewModel(_selectedMaterial.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewMaterial command.
        /// </summary>
        public ICommand CreateNewMaterialCommand { get; }

        /// <summary>
        /// Loads the view model and publishes the Change View event.
        /// </summary>
        public async void ViewModelSelected()
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
            Materials.Clear();
            var materials = await _materialRepository.GetAllAsync();
            foreach (var material in materials)
            {
                Materials.Add(new MaterialWrapper(material));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (MaterialDetailViewModel != null && MaterialDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            MaterialDetailViewModel = _materialDetailViewModelCreator();
            await MaterialDetailViewModel.LoadAsync(id);
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterMaterialSaved(AfterMaterialSavedEventArgs args)
        {
            var item = Materials.SingleOrDefault(p => p.Id == args.Material.Id);

            if (item == null)
            {
                Materials.Add(new MaterialWrapper(args.Material));
                MaterialDetailViewModel = null;
            }
            else
            {
                item.Name = args.Material.Name;
            }
        }

        private void AfterMaterialDeleted(int materialId)
        {
            var item = Materials.SingleOrDefault(m => m.Id == materialId);

            if (item != null)
            {
                Materials.Remove(item);
            }

            MaterialDetailViewModel = null;
        }

        private void OnCreateNewMaterialExecute()
        {
            UpdateDetailViewModel(null);
        }
    }
}