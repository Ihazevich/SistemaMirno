using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class ClientDetailViewModel : DetailViewModelBase, IClientDetailViewModel
    {
        private IClientRepository _clientRepository;
        private ClientWrapper _client;

        public ClientDetailViewModel(
            IClientRepository clientRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Producto", dialogCoordinator)
        {
            _clientRepository = clientRepository;
        }

        public ClientWrapper Client
        {
            get => _client;

            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }
        
        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _clientRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Client = new ClientWrapper(model);
                Client.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            if (IsNew)
            {
                await _clientRepository.AddAsync(Client.Model);
            }
            else
            {
                await _clientRepository.SaveAsync(Client.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ClientViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Client);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            base.OnDeleteExecute();
            await _clientRepository.DeleteAsync(Client.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ClientViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            base.OnCancelExecute();
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ClientViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                Client = new ClientWrapper();
                Client.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                Client.FullName = string.Empty;
                Client.RUC = string.Empty;
                Client.PhoneNumber = string.Empty;
                Client.Address = string.Empty;
                Client.City = string.Empty;
                Client.Department = string.Empty;
                Client.Email = string.Empty;
                Client.IsRetail = false;
                Client.IsWholesaler = false;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

    }
}
