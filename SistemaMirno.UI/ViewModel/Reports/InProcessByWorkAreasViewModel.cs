using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel.Reports
{
    public class InProcessByWorkAreasViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IWorkUnitRepository _workUnitRepository;

        private PropertyGroupDescription _workAreaName = new PropertyGroupDescription("WorkArea.Name");
        private PropertyGroupDescription _colorName = new PropertyGroupDescription("Color.Name");
        private PropertyGroupDescription _materialName = new PropertyGroupDescription("Material.Name");
        private PropertyGroupDescription _productName = new PropertyGroupDescription("Product.Name");

        private bool _includePrice = false;
        private bool _includeResponsible = false;
        private bool _includeSupervisor = false;
        private bool _includeClient = false;

        public InProcessByWorkAreasViewModel(IWorkUnitRepository workUnitRepository,
                    IEventAggregator eventAggregator)
        {
            _workUnitRepository = workUnitRepository;
            _eventAggregator = eventAggregator;

            WorkUnits = new ObservableCollection<WorkUnitWrapper>();

            PrintReportCommand = new DelegateCommand(OnPrintReportExecute);

            WorkUnitsCollection = CollectionViewSource.GetDefaultView(WorkUnits);
            WorkUnitsCollection.GroupDescriptions.Add(_workAreaName);
            WorkUnitsCollection.GroupDescriptions.Add(_productName);
            WorkUnitsCollection.GroupDescriptions.Add(_materialName);
            WorkUnitsCollection.GroupDescriptions.Add(_colorName);
        }

        public ICommand PrintReportCommand { get; }

        public bool IncludePrice
        {
            get => _includePrice;

            set
            {
                _includePrice = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeResponsible
        {
            get => _includeResponsible;

            set
            {
                _includeResponsible = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeSupervisor
        {
            get => _includeSupervisor;

            set
            {
                _includeSupervisor = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeCLient
        {
            get => _includeClient;

            set
            {
                _includeClient = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<WorkUnitWrapper> WorkUnits { get; set; }

        public ICollectionView WorkUnitsCollection { get; set; }

        public override async Task LoadAsync(int? workAreaId)
        {
            WorkUnits.Clear();
            var workUnits = await _workUnitRepository.GetWorkUnitsInProcessAsync();

            foreach (var workUnit in workUnits)
            {
                WorkUnits.Add(new WorkUnitWrapper(workUnit));
            }
        }

        private void OnPrintReportExecute()
        {

        }
    }
}
