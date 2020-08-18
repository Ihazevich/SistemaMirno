// <copyright file="ClientDetailViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class ClientDetailViewModel : DetailViewModelBase, IClientDetailViewModel
    {
        private IClientRepository _clientRepository;
        private ClientWrapper _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDetailViewModel"/> class.
        /// </summary>
        /// <param name="colorRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public ClientDetailViewModel(
            IClientRepository clientRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public ClientWrapper Client
        {
            get
            {
                return _client;
            }

            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? clientId)
        {
            var client = clientId.HasValue
                ? await _clientRepository.GetByIdAsync(clientId.Value)
                : CreateNewClient();

            Client = new ClientWrapper(client);
            Client.PropertyChanged += Client_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (client.Id == 0)
            {
                // This triggers the validation.
                Client.FirstName = string.Empty;
                Client.LastName = string.Empty;
                Client.RUC = string.Empty;
                Client.PhoneNumber = string.Empty;
                Client.Address = string.Empty;
                Client.Department = string.Empty;
                Client.City = string.Empty;
            }
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _clientRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(Client.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Client != null && !Client.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _clientRepository.Remove(Client.Model);
            await _clientRepository.SaveAsync();
            RaiseDataModelDeletedEvent(Client.Model);
        }

        private void Client_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _clientRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Client.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private Client CreateNewClient()
        {
            var client = new Client();
            _clientRepository.Add(client);
            return client;
        }
    }
}
