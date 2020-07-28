using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    public class WorkUnitViewModel : ViewModelBase, IWorkUnitViewModel
    {
        private IWorkUnitDataService _workUnitDataService;
        private IEventAggregator _eventAggregator;
        
        private string _areaName;

        public string AreaName
        {
            get { return _areaName; }
            set
            {
                _areaName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<WorkUnit> WorkUnits { get; set; }

        public WorkUnitViewModel(IWorkUnitDataService workUnitDataService,
            IEventAggregator eventAggregator)
        {
            WorkUnits = new ObservableCollection<WorkUnit>();
            _workUnitDataService = workUnitDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowProductionAreaWorkUnitsEvent>()
                .Subscribe(OnProductionAreaSelected);
        }

        private async void OnProductionAreaSelected(int productionAreaId)
        {
            Console.WriteLine("Selected area {0}", productionAreaId);
            await LoadAsync(productionAreaId);
            _eventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(this);
        }

        public async Task LoadAsync(int productionAreaId)
        {
            WorkUnits.Clear();
            AreaName = await _workUnitDataService.GetProductionAreaName(productionAreaId);
            Console.WriteLine(AreaName);
            var workUnits = new ObservableCollection<WorkUnit>(await _workUnitDataService.GetWorkUnitsByAreaIdAsync(productionAreaId));
            Console.WriteLine(WorkUnits.Count);
            foreach(var workUnit in workUnits)
            {
                WorkUnits.Add(workUnit);
            }
        }
    }
}
