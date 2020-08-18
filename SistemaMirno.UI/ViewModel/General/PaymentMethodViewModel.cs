// <copyright file="PaymentMethodViewModel.cs" company="HazeLabs">
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
    public class PaymentMethodViewModel : ViewModelBase
    {
        private IPaymentMethodRepository _paymentMethodRepository;
        private IMessageDialogService _messageDialogService;
        private PaymentMethodWrapper _selectedPaymentMethod;
        private IPaymentMethodDetailViewModel _paymentMethodDetailViewModel;
        private Func<IPaymentMethodDetailViewModel> _paymentMethodDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentMethodViewModel"/> class.
        /// </summary>
        /// <param name="paymentMethodDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public PaymentMethodViewModel(
            Func<IPaymentMethodDetailViewModel> paymentMethodDetailViewModelCreator,
            IPaymentMethodRepository paymentMethodRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
            : base(eventAggregator)
        {
            _paymentMethodDetailViewModelCreator = paymentMethodDetailViewModelCreator;
            _paymentMethodRepository = paymentMethodRepository;
            _messageDialogService = messageDialogService;

            _eventAggregator.GetEvent<AfterDataModelSavedEvent<PaymentMethod>>()
                .Subscribe(AfterPaymentMethodSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<PaymentMethod>>()
                .Subscribe(AfterPaymentMethodDeleted);

            PaymentMethods = new ObservableCollection<PaymentMethodWrapper>();
            CreateNewPaymentMethodCommand = new DelegateCommand(OnCreateNewPaymentMethodExecute);
        }

        /// <summary>
        /// Gets the Color detail view model.
        /// </summary>
        public IPaymentMethodDetailViewModel PaymentMethodDetailViewModel
        {
            get
            {
                return _paymentMethodDetailViewModel;
            }

            private set
            {
                _paymentMethodDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of PaymentMethods.
        /// </summary>
        public ObservableCollection<PaymentMethodWrapper> PaymentMethods { get; set; }

        /// <summary>
        /// Gets or sets the selected PaymentMethod.
        /// </summary>
        public PaymentMethodWrapper SelectedPaymentMethod
        {
            get
            {
                return _selectedPaymentMethod;
            }

            set
            {
                OnPropertyChanged();
                _selectedPaymentMethod = value;
                if (_selectedPaymentMethod != null)
                {
                    UpdateDetailViewModel(_selectedPaymentMethod.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewColor command.
        /// </summary>
        public ICommand CreateNewPaymentMethodCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync(int? id)
        {
            PaymentMethods.Clear();
            var paymentMethods = await _paymentMethodRepository.GetAllAsync();
            foreach (var paymentMethod in paymentMethods)
            {
                PaymentMethods.Add(new PaymentMethodWrapper(paymentMethod));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (PaymentMethodDetailViewModel != null && PaymentMethodDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            PaymentMethodDetailViewModel = _paymentMethodDetailViewModelCreator();
            await PaymentMethodDetailViewModel.LoadAsync(id);
        }

        private async void AfterPaymentMethodSaved(AfterDataModelSavedEventArgs<PaymentMethod> args)
        {
            var item = PaymentMethods.SingleOrDefault(m => m.Id == args.Model.Id);

            if (item == null)
            {
                PaymentMethods.Add(new PaymentMethodWrapper(args.Model));
                PaymentMethodDetailViewModel = null;
            }
            else
            {
                item.Name = args.Model.Name;
                SelectedPaymentMethod = null;
                PaymentMethodDetailViewModel = null;
            }
        }

        private async void AfterPaymentMethodDeleted(AfterDataModelDeletedEventArgs<PaymentMethod> args)
        {
            var item = PaymentMethods.SingleOrDefault(m => m.Id == args.Model.Id);

            if (item != null)
            {
                PaymentMethods.Remove(item);
            }

            PaymentMethodDetailViewModel = null;
        }

        private void OnCreateNewPaymentMethodExecute()
        {
            UpdateDetailViewModel(null);
        }
    }
}
