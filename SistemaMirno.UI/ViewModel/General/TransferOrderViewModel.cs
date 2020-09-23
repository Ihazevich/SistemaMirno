﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;

namespace SistemaMirno.UI.ViewModel.General
{
    public class TransferOrderViewModel : ViewModelBase
    {
        private readonly ITransferOrderRepository _transferOrderRepository;
        private TransferOrder _selectedTransferOrder;

        private string _datagridTitle;

        public TransferOrderViewModel(
            ITransferOrderRepository transferOrderRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Ordenes de Traslado", dialogCoordinator)
        {
            _transferOrderRepository = transferOrderRepository;

            TransferOrders = new ObservableCollection<TransferOrder>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
            ShowUnconfirmedCommand = new DelegateCommand(OnShowUnconfirmedExecute);
            ShowIncomingCommand = new DelegateCommand(OnShowIncomingExecute);
        }

        private void OnShowIncomingExecute()
        {
            LoadIncomingTransferOrdersAsync().ConfigureAwait(false);
        }

        private void OnShowUnconfirmedExecute()
        {
            LoadUnconfirmedTransferOrdersAsync().ConfigureAwait(false);
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedTransferOrder.Id,
                    ViewModel = nameof(TransferOrderDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedTransferOrder != null;
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(TransferOrderDetailViewModel),
                });
        }

        public ObservableCollection<TransferOrder> TransferOrders { get; }

        public TransferOrder SelectedTransferOrder
        {
            get
            {
                return _selectedTransferOrder;
            }

            set
            {
                OnPropertyChanged();
                _selectedTransferOrder = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public string DatagridTitle
        {
            get => _datagridTitle;

            set
            {
                _datagridTitle = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public ICommand ShowUnconfirmedCommand { get; }

        public ICommand ShowIncomingCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadIncomingTransferOrdersAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                DatagridTitle = "Ordenes sin confirmar";
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private async Task LoadIncomingTransferOrdersAsync()
        {
            TransferOrders.Clear();

            var transferOrders = await _transferOrderRepository.GetAllIncomingAsync(SessionInfo.Branch.Id);

            foreach (var transferOrder in transferOrders)
            {
                Application.Current.Dispatcher.Invoke(() => TransferOrders.Add(transferOrder));
            }

            Application.Current.Dispatcher.Invoke(() => DatagridTitle = "Traslados entrantes sin verificar");
        }

        private async Task LoadUnconfirmedTransferOrdersAsync()
        {
            TransferOrders.Clear();

            var transferOrders = await _transferOrderRepository.GetAllUnconfirmedAsync(SessionInfo.Branch.Id);

            foreach (var transferOrder in transferOrders)
            {
                Application.Current.Dispatcher.Invoke(() => TransferOrders.Add(transferOrder));
            }

            Application.Current.Dispatcher.Invoke(() => DatagridTitle = "Ordenes sin confirmar");
        }
    }
}