// <copyright file="StockViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class StockViewModel : ViewModelBase
    {
        private WorkAreaWrapper _stockWorkArea;

        private WorkUnitWrapper _selectedStockWorkUnit;
        private WorkUnitWrapper _selectedOrderWorkUnit;
        private IWorkUnitRepository _workUnitRepository;

        private PropertyGroupDescription _clientName = new PropertyGroupDescription("Client.Name");
        private PropertyGroupDescription _colorName = new PropertyGroupDescription("Color.Name");
        private PropertyGroupDescription _materialName = new PropertyGroupDescription("Material.Name");
        private PropertyGroupDescription _productName = new PropertyGroupDescription("Product.Name");

        public StockViewModel(IWorkUnitRepository workUnitRepository,
                    IEventAggregator eventAggregator)
            : base(eventAggregator, "Unidades de Trabajo en Stock")
        {
            _workUnitRepository = workUnitRepository;

            StockWorkUnits = new ObservableCollection<WorkUnitWrapper>();
            OrderWorkUnits = new ObservableCollection<WorkUnitWrapper>();

            ViewStockMovementsCommand = new DelegateCommand(OnViewStockMovementsExecute);
            NewSaleCommand = new DelegateCommand(OnNewSaleExecute, CanNewMovementExecute);
            NewMovementCommand = new DelegateCommand(OnNewMovementExecute, CanNewMovementExecute);

            AddWorkUnitCommand = new DelegateCommand(OnAddWorkUnitExecute, CanAddWorkUnitExecute);
            RemoveWorkUnitCommand = new DelegateCommand(OnRemoveWorkUnitExecute, CanRemoveWorkUnitExecute);

            FilterByClientCommand = new DelegateCommand<object>(OnFilterByClientExecute);
            FilterByColorCommand = new DelegateCommand<object>(OnFilterByColorExecute);
            FilterByMaterialCommand = new DelegateCommand<object>(OnFilterByMaterialExecute);
            FilterByProductCommand = new DelegateCommand<object>(OnFilterByProductExecute);

            StockCollection = CollectionViewSource.GetDefaultView(StockWorkUnits);
            OrderCollection = CollectionViewSource.GetDefaultView(OrderWorkUnits);
        }

        public WorkAreaWrapper StockWorkArea
        {
            get => _stockWorkArea;

            set
            {
                _stockWorkArea = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewSaleCommand { get; }

        public ICommand NewMovementCommand { get; }

        public ICommand ViewStockMovementsCommand { get; }

        public ICommand AddWorkUnitCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

        public ICommand FilterByClientCommand { get; }

        public ICommand FilterByColorCommand { get; }

        public ICommand FilterByMaterialCommand { get; }

        public ICommand FilterByProductCommand { get; }

        public ObservableCollection<WorkUnitWrapper> StockWorkUnits { get; set; }

        public ICollectionView StockCollection { get; set; }

        public ObservableCollection<WorkUnitWrapper> OrderWorkUnits { get; set; }

        public ICollectionView OrderCollection { get; set; }

        public WorkUnitWrapper SelectedAreaWorkUnit
        {
            get => _selectedStockWorkUnit;

            set
            {
                _selectedStockWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)AddWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public WorkUnitWrapper SelectedOrderWorkUnit
        {
            get => _selectedOrderWorkUnit;

            set
            {
                _selectedOrderWorkUnit = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveWorkUnitCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? workAreaId)
        {
            StockWorkUnits.Clear();
            StockWorkArea = new WorkAreaWrapper(await _workUnitRepository.GetStockWorkAreaAsync());

            foreach (var workUnit in StockWorkArea.WorkUnits)
            {
                StockWorkUnits.Add(new WorkUnitWrapper(workUnit));
            }
        }

        private void OnViewStockMovementsExecute()
        {
        }

        private void OnNewSaleExecute()
        {
        }

        private void OnAddWorkUnitExecute()
        {
            OrderWorkUnits.Add(SelectedAreaWorkUnit);
            StockWorkUnits.Remove(SelectedAreaWorkUnit);
            ((DelegateCommand)NewMovementCommand).RaiseCanExecuteChanged();
        }

        private void OnRemoveWorkUnitExecute()
        {
            StockWorkUnits.Add(SelectedOrderWorkUnit);
            OrderWorkUnits.Remove(SelectedOrderWorkUnit);
            ((DelegateCommand)NewMovementCommand).RaiseCanExecuteChanged();
        }

        private bool CanRemoveWorkUnitExecute()
        {
            return SelectedOrderWorkUnit != null;
        }

        private bool CanAddWorkUnitExecute()
        {
            return SelectedAreaWorkUnit != null;
        }

        private void OnNewMovementExecute()
        {
        }

        private bool CanNewMovementExecute()
        {
            if (OrderWorkUnits.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnFilterByClientExecute(object isChecked)
        {
            if (((bool?)isChecked).HasValue)
            {
                if (((bool?)isChecked).Value)
                {
                    OrderCollection.GroupDescriptions.Add(_clientName);
                    StockCollection.GroupDescriptions.Add(_clientName);
                }
                else
                {
                    OrderCollection.GroupDescriptions.Remove(_clientName);
                    StockCollection.GroupDescriptions.Remove(_clientName);
                }
            }
        }

        private void OnFilterByColorExecute(object isChecked)
        {
            if (((bool?)isChecked).HasValue)
            {
                if (((bool?)isChecked).Value)
                {
                    OrderCollection.GroupDescriptions.Add(_colorName);
                    StockCollection.GroupDescriptions.Add(_colorName);
                }
                else
                {
                    OrderCollection.GroupDescriptions.Remove(_colorName);
                    StockCollection.GroupDescriptions.Remove(_colorName);
                }
            }
        }

        private void OnFilterByMaterialExecute(object isChecked)
        {
            if (((bool?)isChecked).HasValue)
            {
                if (((bool?)isChecked).Value)
                {
                    OrderCollection.GroupDescriptions.Add(_materialName);
                    StockCollection.GroupDescriptions.Add(_materialName);
                }
                else
                {
                    OrderCollection.GroupDescriptions.Remove(_materialName);
                    StockCollection.GroupDescriptions.Remove(_materialName);
                }
            }
        }

        private void OnFilterByProductExecute(object isChecked)
        {
            if (((bool?)isChecked).HasValue)
            {
                if (((bool?)isChecked).Value)
                {
                    OrderCollection.GroupDescriptions.Add(_productName);
                    StockCollection.GroupDescriptions.Add(_productName);
                }
                else
                {
                    OrderCollection.GroupDescriptions.Remove(_productName);
                    StockCollection.GroupDescriptions.Remove(_productName);
                }
            }
        }
    }
}
