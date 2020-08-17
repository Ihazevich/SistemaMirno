using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Reports;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Reports
{
    public class ProductionByWorkAreaViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IWorkAreaRepository _workAreaRepository;

        private PropertyGroupDescription _workAreaName = new PropertyGroupDescription("WorkArea.Name");
        private PropertyGroupDescription _colorName = new PropertyGroupDescription("Color.Name");
        private PropertyGroupDescription _materialName = new PropertyGroupDescription("Material.Name");
        private PropertyGroupDescription _productName = new PropertyGroupDescription("Product.Name");

        private WorkAreaWrapper _selectedWorkArea;

        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today;

        private bool _areaPickerEnabled = false;

        public ProductionByWorkAreaViewModel(
                    IWorkAreaRepository workAreaRepository,
                    IEventAggregator eventAggregator)
        {
            _workAreaRepository = workAreaRepository;
            _eventAggregator = eventAggregator;

            WorkAreas = new ObservableCollection<WorkAreaWrapper>();
            WorkUnits = new ObservableCollection<WorkUnitWrapper>();

            PrintReportCommand = new DelegateCommand(OnPrintReportExecute);

            WorkUnitsCollection = CollectionViewSource.GetDefaultView(WorkUnits);
            WorkUnitsCollection.GroupDescriptions.Add(_workAreaName);
            WorkUnitsCollection.GroupDescriptions.Add(_productName);
            WorkUnitsCollection.GroupDescriptions.Add(_materialName);
            WorkUnitsCollection.GroupDescriptions.Add(_colorName);
        }

        public ICommand PrintReportCommand { get; }

        /// <summary>
        /// Gets or sets the selected work area.
        /// </summary>
        public WorkAreaWrapper SelectedWorkArea
        {
            get => _selectedWorkArea;

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
                SelectWorkUnits();
            }
        }

        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; set; }

        public ObservableCollection<WorkUnitWrapper> WorkUnits { get; set; }

        public ICollectionView WorkUnitsCollection { get; set; }

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
                ValidateDates();
            }
        }

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
                ValidateDates();
            }
        }

        public bool AreaPickerEnabled
        {
            get => _areaPickerEnabled;

            set
            {
                _areaPickerEnabled = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int? workAreaId)
        {
            WorkAreas.Clear();
            var workAreas = await _workAreaRepository.GetAllAsync();

            foreach (var workArea in workAreas)
            {
                WorkAreas.Add(new WorkAreaWrapper(workArea));
            }
        }

        private void OnPrintReportExecute()
        {

        }

        private async void SelectWorkUnits()
        {
            WorkUnits.Clear();
            if (SelectedWorkArea.IncomingWorkOrders != null)
            {
                foreach (WorkOrder workOrder in SelectedWorkArea.IncomingWorkOrders)
                {
                    if (workOrder.StartTime > StartDate && workOrder.StartTime < EndDate)
                    {
                        foreach (WorkOrderUnit workOrderUnit in workOrder.WorkOrderUnits)
                        {
                            WorkUnits.Add(new WorkUnitWrapper(workOrderUnit.WorkUnit));
                        }
                    }
                }
            }
        }

        private void ValidateDates()
        {
            bool areDatesValid = false;

            if (StartDate != null && EndDate != null)
            {
                if (StartDate <= DateTime.Today && EndDate <= DateTime.Today)
                {
                    if (EndDate >= StartDate)
                    {
                        areDatesValid = true;
                    }
                }
            }

            AreaPickerEnabled = areDatesValid;
        }
    }
}
