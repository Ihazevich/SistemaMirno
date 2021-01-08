using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Wrapper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ProductSearchViewModel : ViewModelBase
    {
        private readonly PropertyGroupDescription _productName = new PropertyGroupDescription("Model.Description");
        private readonly PropertyGroupDescription _areaName = new PropertyGroupDescription("Model.CurrentWorkArea.Name");
        private readonly IWorkUnitRepository _workUnitRepository;
        private string _workAreaWorkUnitProductFilter;

        public ProductSearchViewModel(
            IWorkUnitRepository workUnitRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Busqueda de productos en fabrica", dialogCoordinator)
        {
            _workUnitRepository = workUnitRepository;

            WorkAreaWorkUnits = new ObservableCollection<WorkUnitWrapper>();

            WorkAreaCollectionView = CollectionViewSource.GetDefaultView(WorkAreaWorkUnits);
            WorkAreaCollectionView.GroupDescriptions.Add(_productName);
            WorkAreaCollectionView.GroupDescriptions.Add(_areaName);

            WorkAreaWorkUnitProductFilter = string.Empty;
        }


        public ICommand ShowWorkUnitDetailsCommand { get; }

        public long TotalProductionValue { get; set; }

        public long TotalRetailPrice { get; set; }

        public long TotalWholesalerPrice { get; set; }

        public ICollectionView WorkAreaCollectionView { get; }

        public string WorkAreaWorkUnitProductFilter
        {
            get => _workAreaWorkUnitProductFilter;

            set
            {
                _workAreaWorkUnitProductFilter = value;
                OnPropertyChanged();
                SearchWorkUnits(_workAreaWorkUnitProductFilter);
            }
        }

        public ObservableCollection<WorkUnitWrapper> WorkAreaWorkUnits { get; }

        public ICollectionView WorkOrderCollectionView { get; }

        public ObservableCollection<WorkUnitWrapper> WorkOrderWorkUnits { get; }

        public override async Task LoadAsync(int? id = null)
        {
            WorkAreaWorkUnits.Clear();

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private async Task SearchWorkUnits(string name)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Visible;
            });

            WorkAreaWorkUnits.Clear();

            if (!string.IsNullOrWhiteSpace(name))
            {
                var workUnits = await _workUnitRepository.GetAllWorkUnitsByNameAsync(name);

                TotalProductionValue = 0;
                TotalWholesalerPrice = 0;
                TotalRetailPrice = 0;

                if (workUnits != null)
                {
                    foreach (var workUnit in workUnits)
                    {
                        TotalProductionValue += workUnit.Product.ProductionValue;
                        TotalWholesalerPrice += workUnit.Product.WholesalerPrice;
                        TotalRetailPrice += workUnit.Product.RetailPrice;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            WorkAreaWorkUnits.Add(new WorkUnitWrapper(workUnit));
                        });
                    }
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }
    }
}
