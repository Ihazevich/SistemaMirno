using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
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
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _areDatesValid;
        private bool _seeDetailsButtonEnabled;
        private string _clientSearch;

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

            SalesCollection = CollectionViewSource.GetDefaultView(Sales);

            CreateNewSaleCommand = new DelegateCommand(OnCreateNewSaleExecute);
            PrintReportCommand = new DelegateCommand(OnPrintReportExecute);

            MonthlySeriesCollection = new SeriesCollection();
            DailySeriesCollection = new SeriesCollection();

            MonthlyLabels = new[]
            {
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre",
            };

            var today = DateTime.Today;

            StartDate = new DateTime(today.Year, today.Month, 1);
            EndDate = StartDate.AddMonths(1).AddTicks(-1);

            SeeDetailsButtonEnabled = false;
        }

        public string ClientSearch
        {
            get => _clientSearch;

            set
            {
                _clientSearch = value;
                OnPropertyChanged();
                FilterByClient(_clientSearch);
            }
        }

        private void FilterByClient(string clientSearch)
        {
            clientSearch = clientSearch.ToLower();
            SalesCollection.Filter = o =>
            {
                Sale s = o as Sale;
                return s.Requisition.Client.FirstName.ToLower().Contains(clientSearch)
                       || s.Requisition.Client.LastName.ToLower().Contains(clientSearch);
            };
        }

        public bool SeeDetailsButtonEnabled
        {
            get => _seeDetailsButtonEnabled;

            set
            {
                _seeDetailsButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool AreDatesValid
        {
            get => _areDatesValid;

            set
            {
                _areDatesValid = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView SalesCollection { get; set; }

        public SeriesCollection MonthlySeriesCollection { get; set; }

        public SeriesCollection DailySeriesCollection { get; set; }

        public ICommand PrintReportCommand { get; }

        /// <summary>
        /// Gets or sets the start date for the report.
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;

            set
            {
                _startDate = value;
                OnPropertyChanged();
                if (ValidateDates())
                {
                    SelectSales();
                }
            }
        }

        public string[] MonthlyLabels { get; set; }

        /// <summary>
        /// Gets or sets the end date for the report.
        /// </summary>
        public DateTime EndDate
        {
            get => _endDate;

            set
            {
                _endDate = value;
                OnPropertyChanged();
                if (ValidateDates())
                {
                    SelectSales();
                }
            }
        }


        private bool ValidateDates()
        {
            AreDatesValid = false;

            if (StartDate != null && EndDate != null)
            {
                if (DateTime.Compare(StartDate, DateTime.Today) <= 0 && DateTime.Compare(EndDate, StartDate) >= 0)
                {
                    AreDatesValid = true;
                }
            }

            return AreDatesValid;
        }

        private async void SelectSales()
        {
            Sales.Clear();

            var sales = await _saleRepository.GetAllBetweenTwoDatesAsync(StartDate, EndDate);

            foreach (var sale in sales)
            {
                Sales.Add(new SaleWrapper(sale));
            }

            CalculateMonthlySales();
            CalculateDailySales();
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
                    SeeDetailsButtonEnabled = true;
                }
                else
                {
                    SeeDetailsButtonEnabled = false;
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

        private void OnPrintReportExecute()
        {

        }

        private void CalculateMonthlySales()
        {
            int[] monthlySales = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            foreach (var sale in Sales)
            {
                if (sale.Requisition.RequestedDate.Year == DateTime.Today.Year)
                {
                    monthlySales[sale.Requisition.RequestedDate.Month - 1] += sale.Total;
                }
            }

            LineSeries lineSeries = new LineSeries
            {
                Title = "Total Ventas",
                Values = new ChartValues<int>(monthlySales),
                DataLabels = true,
                LabelPoint = point => string.Format("{0:n0}", point.Y) + " Gs.",
            };

            MonthlySeriesCollection.Clear();
            MonthlySeriesCollection.Add(lineSeries);
        }

        private void CalculateDailySales()
        {
            // Make a list to store the current month daily productions and one to store the cummulative production
            var dailySales = new List<long>(new long[DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)]);
            var dailyCummulative = new List<long>(new long[DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)]);

            foreach (var sale in Sales)
            {
                if (sale.Requisition.RequestedDate.Month == DateTime.Today.Month
                    && sale.Requisition.RequestedDate.Year == DateTime.Today.Year)
                {
                    dailySales[sale.Requisition.RequestedDate.Day - 1] += sale.Total;
                }
            }

            long cummulative = 0;
            for (int i = 0; i < dailyCummulative.Count; i++)
            {
                cummulative += dailySales[i];
                dailyCummulative[i] = cummulative;
            }

            LineSeries lineSeriesDaily = new LineSeries
            {
                Title = "Venta diaria",
                Values = new ChartValues<long>(dailySales),
                LabelPoint = point => string.Format("{0:n0}", point.Y) + " Gs.",
            };

            LineSeries lineSeriesCummulative = new LineSeries
            {
                Title = "Venta acumulada",
                Values = new ChartValues<long>(dailyCummulative),
                LabelPoint = point => string.Format("{0:n0}", point.Y) + " Gs.",
            };

            DailySeriesCollection.Clear();
            DailySeriesCollection.Add(lineSeriesDaily);
            DailySeriesCollection.Add(lineSeriesCummulative);
        }
    }
}
