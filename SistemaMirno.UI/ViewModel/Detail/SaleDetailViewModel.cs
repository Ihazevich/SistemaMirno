using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class SaleDetailViewModel : DetailViewModelBase, ISaleDetailViewModel
    {
        private ISaleRepository _saleRepository;
        private SaleWrapper _sale;
        private bool _isNewSale;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDetailViewModel"/> class.
        /// </summary>
        /// <param name="colorRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public SaleDetailViewModel(
            ISaleRepository saleRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _saleRepository = saleRepository;
        }

        public bool IsNewSale
        {
            get => _isNewSale;

            set
            {
                _isNewSale = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public SaleWrapper Sale
        {
            get
            {
                return _sale;
            }

            set
            {
                _sale = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? saleId)
        {
            var sale = saleId.HasValue
                ? await _saleRepository.GetByIdAsync(saleId.Value)
                : CreateNewSale();

            Sale = new SaleWrapper(sale);
            Sale.PropertyChanged += Client_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (sale.Id == 0)
            {
                // This triggers the validation.
                // TODO: Trigger validation
            }
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _saleRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(Sale.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Sale != null && !Sale.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _saleRepository.Remove(Sale.Model);
            await _saleRepository.SaveAsync();
            RaiseDataModelDeletedEvent(Sale.Model);
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(SaleViewModel) });
        }

        private void Client_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _saleRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Sale.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private Sale CreateNewSale()
        {
            var sale = new Sale();
            _saleRepository.Add(sale);
            return sale;
        }
    }
}
