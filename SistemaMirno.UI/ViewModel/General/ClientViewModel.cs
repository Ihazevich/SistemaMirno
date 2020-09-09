using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ClientViewModel : ViewModelBase
    {
        private IClientRepository _clientRepository;
        private Func<IClientRepository> _clientRepositoryCreator;
        private ClientWrapper _selectedClient;

        public ClientViewModel(
            Func<IClientRepository> clientRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Clientes", dialogCoordinator)
        {
            _clientRepositoryCreator = clientRepositoryCreator;

            Clients = new ObservableCollection<ClientWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedClient.Id,
                    ViewModel = nameof(ClientDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedClient != null;
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ClientDetailViewModel),
                });
        }

        public ObservableCollection<ClientWrapper> Clients { get; }

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
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Clients.Clear();
            _clientRepository = _clientRepositoryCreator();

            var clients = await _clientRepository.GetAllAsync();

            foreach (var client in clients)
            {
                Application.Current.Dispatcher.Invoke(() => Clients.Add(new ClientWrapper(client)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }
    }
}
