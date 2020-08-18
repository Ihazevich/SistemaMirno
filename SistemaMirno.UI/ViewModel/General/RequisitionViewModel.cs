// <copyright file="RequisitionViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class RequisitionViewModel : ViewModelBase
    {
        private IRequisitionRepository _requisitionRepository;
        private IMessageDialogService _messageDialogService;
        private RequisitionWrapper _selectedRequisition;
        private IRequisitionDetailViewModel _requisitionDetailViewModel;
        private Func<IRequisitionDetailViewModel> _requisitionDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequisitionViewModel"/> class.
        /// </summary>
        /// <param name="requisitionDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public RequisitionViewModel(
            Func<IRequisitionDetailViewModel> requisitionDetailViewModelCreator,
            IRequisitionRepository requisitionRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
            : base(eventAggregator)
        {
            _requisitionDetailViewModelCreator = requisitionDetailViewModelCreator;
            _requisitionRepository = requisitionRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;

            Requisitions = new ObservableCollection<RequisitionWrapper>();
        }

        /// <summary>
        /// Gets the Client detail view model.
        /// </summary>
        public IRequisitionDetailViewModel RequisitionDetailViewModel
        {
            get
            {
                return _requisitionDetailViewModel;
            }

            private set
            {
                _requisitionDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Clients.
        /// </summary>
        public ObservableCollection<RequisitionWrapper> Requisitions { get; set; }

        /// <summary>
        /// Gets or sets the selected Color.
        /// </summary>
        public RequisitionWrapper SelectedRequisition
        {
            get
            {
                return _selectedRequisition;
            }

            set
            {
                OnPropertyChanged();
                _selectedRequisition = value;
                if (_selectedRequisition != null)
                {
                    UpdateDetailViewModel(_selectedRequisition.Id);
                }
            }
        }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync(int? id)
        {
            Requisitions.Clear();
            var requisitions = await _requisitionRepository.GetAllAsync();
            foreach (var requisition in requisitions)
            {
                Requisitions.Add(new RequisitionWrapper(requisition));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (RequisitionDetailViewModel != null && RequisitionDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            RequisitionDetailViewModel = _requisitionDetailViewModelCreator();
            await RequisitionDetailViewModel.LoadAsync(id);
        }
    }
}
