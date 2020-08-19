using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    public class SaleViewModel : ViewModelBase
    {
        private ISaleRepository _saleRepository;
        private IMessageDialogService _messageDialogService;
        private SaleWrapper _selectedSale;
        private ISaleDetailViewModel _saleDetailViewModel;
        private Func<ISaleDetailViewModel> _saleDetailViewModelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleViewModel"/> class.
        /// </summary>
        /// <param name="clientDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
        public SaleViewModel(
            Func<ISaleDetailViewModel> saleDetailViewModelCreator,
            ISaleRepository saleRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
            : base(eventAggregator)
        {
            _saleDetailViewModelCreator = saleDetailViewModelCreator;
            _saleRepository = saleRepository;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<Sale>>()
                .Subscribe(AfterSaleSaved);
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<Sale>>()
                .Subscribe(AfterSaleDeleted);

            Sales = new ObservableCollection<SaleWrapper>();
            CreateNewSaleCommand = new DelegateCommand(OnCreateNewSaleExecute);
        }

        /// <summary>
        /// Gets the Client detail view model.
        /// </summary>
        public ISaleDetailViewModel SaleDetailViewModel
        {
            get
            {
                return _saleDetailViewModel;
            }

            private set
            {
                _saleDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of Clients.
        /// </summary>
        public ObservableCollection<SaleWrapper> Sales { get; set; }

        /// <summary>
        /// Gets or sets the selected Color.
        /// </summary>
        public SaleWrapper SelectedSale
        {
            get
            {
                return _selectedSale;
            }

            set
            {
                OnPropertyChanged();
                _selectedSale = value;
                if (_selectedSale != null)
                {
                    UpdateDetailViewModel(_selectedSale.Id);
                }
            }
        }

        /// <summary>
        /// Gets the CreateNewColor command.
        /// </summary>
        public ICommand CreateNewSaleCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync(int? id)
        {
            Sales.Clear();
            var sales = await _saleRepository.GetAllAsync();
            foreach (var sale in sales)
            {
                Sales.Add(new SaleWrapper(sale));
            }
        }

        private async void UpdateDetailViewModel(int? id)
        {
            if (SaleDetailViewModel != null && SaleDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog(
                    "Ha realizado cambios, si selecciona otro item estos cambios seran perdidos. ¿Esta seguro?",
                    "Pregunta");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            SaleDetailViewModel = _saleDetailViewModelCreator();
            await SaleDetailViewModel.LoadAsync(id);
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        private void AfterSaleSaved(AfterDataModelSavedEventArgs<Sale> args)
        {
            var item = Sales.SingleOrDefault(s => s.Id == args.Model.Id);

            if (item == null)
            {
                Sales.Add(new SaleWrapper(args.Model));
                SaleDetailViewModel = null;
            }
        }

        private void AfterSaleDeleted(AfterDataModelDeletedEventArgs<Sale> args)
        {
            var item = Sales.SingleOrDefault(s => s.Id == args.Model.Id);

            if (item != null)
            {
                Sales.Remove(item);
            }

            SaleDetailViewModel = null;
        }

        private void OnCreateNewSaleExecute()
        {
            UpdateDetailViewModel(null);
        }
    }
}
