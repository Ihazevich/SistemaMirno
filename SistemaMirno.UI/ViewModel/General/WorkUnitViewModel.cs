﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    /// <summary>
    /// View model for Work Units.
    /// </summary>
    public class WorkUnitViewModel : ViewModelBase, IWorkUnitViewModel
    {
        private int _areaId;
        private string _areaName;
        private WorkUnitWrapper _selectedAreaWorkUnit;
        private WorkUnitWrapper _selectedOrderWorkUnit;
        private IEventAggregator _eventAggregator;
        private IWorkUnitRepository _workUnitRepository;

        private PropertyGroupDescription _clientName = new PropertyGroupDescription("Client.Name");
        private PropertyGroupDescription _colorName = new PropertyGroupDescription("Color.Name");
        private PropertyGroupDescription _materialName = new PropertyGroupDescription("Material.Name");
        private PropertyGroupDescription _productName = new PropertyGroupDescription("Product.Name");

        public WorkUnitViewModel(IWorkUnitRepository workUnitRepository,
                    IEventAggregator eventAggregator)
        {
            _workUnitRepository = workUnitRepository;
            _eventAggregator = eventAggregator;

            AreaWorkUnits = new ObservableCollection<WorkUnitWrapper>();
            OrderWorkUnits = new ObservableCollection<WorkUnitWrapper>();

            OpenWorkOrderViewCommand = new DelegateCommand(OnOpenWorkOrderViewExecute);
            NewWorkOrderCommand = new DelegateCommand(OnNewWorkOrderExecute);
            MoveToWorkAreaCommand = new DelegateCommand(OnMoveToWorkAreaExecute, CanMoveToWorkAreaExecute);

            AddWorkUnitCommand = new DelegateCommand(OnAddWorkUnitExecute, CanAddWorkUnitExecute);
            RemoveWorkUnitCommand = new DelegateCommand(OnRemoveWorkUnitExecute, CanRemoveWorkUnitExecute);

            FilterByClientCommand = new DelegateCommand<object>(OnFilterByClientExecute);
            FilterByColorCommand = new DelegateCommand<object>(OnFilterByColorExecute);
            FilterByMaterialCommand = new DelegateCommand<object>(OnFilterByMaterialExecute);
            FilterByProductCommand = new DelegateCommand<object>(OnFilterByProductExecute);

            AreaCollection = CollectionViewSource.GetDefaultView(AreaWorkUnits);
            OrderCollection = CollectionViewSource.GetDefaultView(OrderWorkUnits);
        }

        /// <summary>
        /// Gets or sets the production area name for the view.
        /// </summary>
        public string AreaName
        {
            get
            {
                return _areaName;
            }

            set
            {
                _areaName = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewWorkOrderCommand { get; }

        public ICommand MoveToWorkAreaCommand { get; }

        public ICommand OpenWorkOrderViewCommand { get; }

        public ICommand AddWorkUnitCommand { get; }

        public ICommand RemoveWorkUnitCommand { get; }

        public ICommand FilterByClientCommand { get; }

        public ICommand FilterByColorCommand { get; }

        public ICommand FilterByMaterialCommand { get; }

        public ICommand FilterByProductCommand { get; }

        public ObservableCollection<WorkUnitWrapper> AreaWorkUnits { get; set; }

        public ICollectionView AreaCollection { get; set; }

        public ObservableCollection<WorkUnitWrapper> OrderWorkUnits { get; set; }

        public ICollectionView OrderCollection { get; set; }

        public WorkUnitWrapper SelectedAreaWorkUnit
        {
            get => _selectedAreaWorkUnit;

            set
            {
                _selectedAreaWorkUnit = value;
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
            if (workAreaId.HasValue)
            {
                AreaWorkUnits.Clear();
                AreaName = await _workUnitRepository.GetWorkAreaNameAsync(workAreaId.Value);
                _areaId = workAreaId.Value;
                var workUnits = await _workUnitRepository.GetByAreaIdAsync(workAreaId.Value);

                foreach (var workUnit in workUnits)
                {
                    AreaWorkUnits.Add(new WorkUnitWrapper(workUnit));
                }
            }
        }

        private void OnOpenWorkOrderViewExecute()
        {
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(WorkOrderViewModel), Id = _areaId });
        }

        private void OnNewWorkOrderExecute()
        {
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(WorkOrderDetailViewModel), Id = _areaId });
        }

        private void OnAddWorkUnitExecute()
        {
            OrderWorkUnits.Add(SelectedAreaWorkUnit);
            AreaWorkUnits.Remove(SelectedAreaWorkUnit);
            ((DelegateCommand)MoveToWorkAreaCommand).RaiseCanExecuteChanged();
        }

        private void OnRemoveWorkUnitExecute()
        {
            AreaWorkUnits.Add(SelectedOrderWorkUnit);
            OrderWorkUnits.Remove(SelectedOrderWorkUnit);
            ((DelegateCommand)MoveToWorkAreaCommand).RaiseCanExecuteChanged();
        }

        private bool CanRemoveWorkUnitExecute()
        {
            return SelectedOrderWorkUnit != null;
        }

        private bool CanAddWorkUnitExecute()
        {
            return SelectedAreaWorkUnit != null;
        }

        private void OnMoveToWorkAreaExecute()
        {
            throw new NotImplementedException();
        }

        private bool CanMoveToWorkAreaExecute()
        {
            return OrderWorkUnits.Count > 0;
        }

        private void OnFilterByClientExecute(object isChecked)
        {
            if (((bool?)isChecked).HasValue)
            {
                if (((bool?)isChecked).Value)
                {
                    OrderCollection.GroupDescriptions.Add(_clientName);
                    AreaCollection.GroupDescriptions.Add(_clientName);
                }
                else
                {
                    OrderCollection.GroupDescriptions.Remove(_clientName);
                    AreaCollection.GroupDescriptions.Remove(_clientName);
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
                    AreaCollection.GroupDescriptions.Add(_colorName);
                }
                else
                {
                    OrderCollection.GroupDescriptions.Remove(_colorName);
                    AreaCollection.GroupDescriptions.Remove(_colorName);
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
                    AreaCollection.GroupDescriptions.Add(_materialName);
                }
                else
                {
                    OrderCollection.GroupDescriptions.Remove(_materialName);
                    AreaCollection.GroupDescriptions.Remove(_materialName);
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
                    AreaCollection.GroupDescriptions.Add(_productName);
                }
                else
                {
                    OrderCollection.GroupDescriptions.Remove(_productName);
                    AreaCollection.GroupDescriptions.Remove(_productName);
                }
            }
        }
    }
}