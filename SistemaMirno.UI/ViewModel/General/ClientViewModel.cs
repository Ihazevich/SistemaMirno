// <copyright file="ClientViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Services;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ClientViewModel : ViewModelBase, IClientViewModel
    {
        private IClientRepository _clientRepository;
        private IMessageDialogService _messageDialogService;
        private ClientWrapper _selectedClient;
        private IClientDetailViewModel _clientDetailViewModel;
        private Func<IClientDetailViewModel> _clientDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientViewModel"/> class.
        /// </summary>
        /// <param name="clientDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public ClientViewModel(
            Func<IClientDetailViewModel> clientDetailViewModelCreator,
            IClientRepository clientRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
            : base(eventAggregator, "Clientes")
        {
            _clientDetailViewModelCreator = clientDetailViewModelCreator;
            _clientRepository = clientRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<Client>>()
                .Subscribe(AfterClientSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<Client>>()
                .Subscribe(AfterClientDeleted);

            Clients = new ObservableCollection<ClientWrapper>();
            CreateNewClientCommand = new DelegateCommand(OnCreateNewClientExecute);
        }

        /// <summary>
        /// Gets the Client detail view model.
        /// </summary>
        public IClientDetailViewModel ClientDetailViewModel
        {
            get
            {
                return _clientDetailViewModel;
            }

            private set
            {
                _clientDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Clients.
        /// </summary>
        public ObservableCollection<ClientWrapper> Clients { get; set; }

        /// <summary>
        /// Gets or sets the selected Color.
        /// </summary>
        public ClientWrapper SelectedClient
        {
            get
            {
                return _selectedClient;
            }

            set
            {
                OnPropertyChanged();
                _selectedClient = value;
                if (_selectedClient != null)
                {
                    UpdateDetailViewModel(_selectedClient.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewColor command.
        /// </summary>
        public ICommand CreateNewClientCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync(int? id)
        {
            Clients.Clear();
            var clients = await _clientRepository.GetAllAsync();
            foreach (var client in clients)
            {
                Clients.Add(new ClientWrapper(client));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (ClientDetailViewModel != null && ClientDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            ClientDetailViewModel = _clientDetailViewModelCreator();
            ClientDetailViewModel.LoadAsync(id);
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        private void AfterClientSaved(AfterDataModelSavedEventArgs<Client> args)
        {
            var item = Clients.SingleOrDefault(c => c.Id == args.Model.Id);

            if (item == null)
            {
                Clients.Add(new ClientWrapper(args.Model));
                ClientDetailViewModel = null;
            }
            else
            {
                item.FirstName = args.Model.FirstName;
            }
        }

        private void AfterClientDeleted(AfterDataModelDeletedEventArgs<Client> args)
        {
            var item = Clients.SingleOrDefault(m => m.Id == args.Model.Id);

            if (item != null)
            {
                Clients.Remove(item);
            }

            ClientDetailViewModel = null;
        }

        private void OnCreateNewClientExecute()
        {
            UpdateDetailViewModel(null);
        }
    }
}
