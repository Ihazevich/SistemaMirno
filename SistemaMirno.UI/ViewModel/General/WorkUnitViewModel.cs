using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class WorkUnitViewModel : ViewModelBase, IWorkUnitViewModel
    {
        private readonly Func<IWorkUnitRepository> _workAreaRepositoryCreator;
        private IWorkUnitRepository _workUnitRepository;
        private WorkUnitWrapper _selectedWorkAreaWorkUnit;
        private WorkUnitWrapper _selectedWorkOrderWorkUnit;

        public WorkUnitViewModel(
            Func<IWorkUnitRepository> workUnitRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Unidades de Trabajo en Area", dialogCoordinator)
        {
            _workAreaRepositoryCreator = workUnitRepositoryCreator;
            WorkAreaWorkUnits = new ObservableCollection<WorkUnitWrapper>();
            WorkOrderWorkUnits = new ObservableCollection<WorkUnitWrapper>();
        }
        
        public ObservableCollection<WorkUnitWrapper> WorkAreaWorkUnits { get; }

        public ObservableCollection<WorkUnitWrapper> WorkOrderWorkUnits { get; }

        public ObservableCollection<WorkAreaConnectionWrapper> WorkAreaConnections { get; set; }

        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                WorkAreaWorkUnits.Clear();
                _workUnitRepository = _workAreaRepositoryCreator();

                var workUnits = await _workUnitRepository.GetAllWorkUnitsCurrentlyInWorkArea(id.Value);

                foreach (var workUnit in workUnits)
                {
                    Application.Current.Dispatcher.Invoke(() => WorkAreaWorkUnits.Add(new WorkUnitWrapper(workUnit)));
                }

                foreach (var connection in WorkAreaWorkUnits.First().Model.CurrentWorkArea.OutgoingConnections)
                {
                    Application.Current.Dispatcher.Invoke(() => WorkAreaConnections.Add(new WorkAreaConnectionWrapper(connection)));
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    ProgressVisibility = Visibility.Collapsed;
                    ViewVisibility = Visibility.Visible;
                });
            }
            else
            {
                EventAggregator.GetEvent<ChangeViewEvent>()
                    .Publish(new ChangeViewEventArgs());
            }
        }
    }
}
