// <copyright file="PaymentMethodDetailViewModel.cs" company="HazeLabs">
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
    public class PaymentMethodDetailViewModel : DetailViewModelBase, IPaymentMethodDetailViewModel
    {
        private IPaymentMethodRepository _paymentMethodRepository;
        private PaymentMethodWrapper _paymentMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentMethodDetailViewModel"/> class.
        /// </summary>
        /// <param name="colorRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public PaymentMethodDetailViewModel(
            IPaymentMethodRepository paymentMethodRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator, "Detalles de Metodo de Pago")
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public PaymentMethodWrapper PaymentMethod
        {
            get
            {
                return _paymentMethod;
            }

            set
            {
                _paymentMethod = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? methodId)
        {
            var paymentMethod = methodId.HasValue
                ? await _paymentMethodRepository.GetByIdAsync(methodId.Value)
                : CreateNewPaymentMethod();

            PaymentMethod = new PaymentMethodWrapper(paymentMethod);
            PaymentMethod.PropertyChanged += PaymentMethod_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (paymentMethod.Id == 0)
            {
                // This triggers the validation.
                PaymentMethod.Name = string.Empty;
            }
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _paymentMethodRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(PaymentMethod.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return PaymentMethod != null && !PaymentMethod.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _paymentMethodRepository.Remove(PaymentMethod.Model);
            await _paymentMethodRepository.SaveAsync();
            RaiseDataModelDeletedEvent(PaymentMethod.Model);
        }

        private void PaymentMethod_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _paymentMethodRepository.HasChanges();
            }

            if (e.PropertyName == nameof(PaymentMethod.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private PaymentMethod CreateNewPaymentMethod()
        {
            var paymentMethod = new PaymentMethod();
            _paymentMethodRepository.Add(paymentMethod);
            return paymentMethod;
        }
    }
}
