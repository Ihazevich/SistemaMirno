using System.Collections.ObjectModel;
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

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            TransferOrders.Clear();

            var transferOrders = await _transferOrderRepository.GetAllAsync();

            foreach (var transferOrder in transferOrders)
            {
                Application.Current.Dispatcher.Invoke(() => TransferOrders.Add(transferOrder));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }
    }
}
