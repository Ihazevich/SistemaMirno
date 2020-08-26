using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Main
{
    /// <summary>
    /// Production areas navigation view model class.
    /// </summary>
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private bool _navigationEnabled = true;
        private readonly IWorkAreaRepository _workAreaRepository;
        private WorkAreaWrapper _selectedWorkArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaRepository">The work areas repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="dialogCoordinator">The dialog coordinator.</param>
        public NavigationViewModel(
            IWorkAreaRepository workAreaRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base (eventAggregator, "Navegacion", dialogCoordinator)
        {
            _workAreaRepository = workAreaRepository;
            WorkAreas = new ObservableCollection<WorkAreaWrapper>();

            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Subscribe(ChangeNavigation);
        }

        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; }

        private void GetSessionInfo(SessionInfo obj)
        {
            SessionInfo = obj;
        }

        public bool NavigationEnabled
        {
            get => _navigationEnabled;

            set
            {
                _navigationEnabled = value;
                OnPropertyChanged();
            }
        }

        private void ChangeNavigation(bool arg)
        {
            NavigationEnabled = arg;
            if (arg)
            {
                SelectedWorkArea = null;
            }
        }

        /// <summary>
        /// Gets or sets the selected work area.
        /// </summary>
        
        public WorkAreaWrapper SelectedWorkArea
        {
            get
            {
                return _selectedWorkArea;
            }

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
                if (_selectedWorkArea == null)
                {
                    return;
                }

                // If the work area name is the first or last, then redirect to the specialized views of those areas instead.
                if (_selectedWorkArea.IsFirst)
                {
                    // TODO: Open requisitions view
                }
                else if (_selectedWorkArea.IsLast)
                {
                    // TODO: Open stock view
                }
                else
                {
                    EventAggregator.GetEvent<ChangeViewEvent>()
                        .Publish(new ChangeViewEventArgs
                        {
                            Id = _selectedWorkArea.Id,
                            ViewModel = nameof(WorkUnitViewModel),
                        });
                }

                EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                    .Publish(false);
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                var workAreas = await _workAreaRepository.GetAllWorkAreasFromBranchAsync(id.Value);

                workAreas.Sort((a, b) => a.Position.CompareTo(b.Position));

                foreach (var workArea in workAreas)
                {
                    Application.Current.Dispatcher.Invoke(() => WorkAreas.Add(new WorkAreaWrapper(workArea)));
                }
            }
        }
    }
}