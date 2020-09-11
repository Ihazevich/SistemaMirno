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
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class DeliveryViewModel : ViewModelBase
    {
        private IDeliveryRepository _deliveryRepository;
        private Delivery _selectedDelivery;

        public DeliveryViewModel(
            IDeliveryRepository deliveryRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Entregas", dialogCoordinator)
        {
            _deliveryRepository = deliveryRepository;

            Deliveries = new ObservableCollection<Delivery>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedDelivery.Id,
                    ViewModel = nameof(ColorDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedDelivery != null;
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(DeliveryOrderDetailViewModel),
                });
        }

        public ObservableCollection<Delivery> Deliveries { get; }

        public Delivery SelectedDelivery
        {
            get
            {
                return _selectedDelivery;
            }

            set
            {
                OnPropertyChanged();
                _selectedDelivery = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Deliveries.Clear();

            var deliveries = await _deliveryRepository.GetAllAsync();

            foreach (var delivery in deliveries)
            {
                Application.Current.Dispatcher.Invoke(() => Deliveries.Add(delivery));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }
    }
}
