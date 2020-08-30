using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class WorkAreaMovementViewModel : ViewModelBase, IWorkAreaMovementViewModel
    {
        private IWorkAreaMovementRepository _workAreaMovementRepository;
        private WorkArea _selectedWorkArea;
        private DateTime _selectedDate;

        public WorkAreaMovementViewModel(
            IWorkAreaMovementRepository workAreaMovementRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Movimientos de Area", dialogCoordinator)
        {
            _workAreaMovementRepository = workAreaMovementRepository;

            WorkAreas = new ObservableCollection<WorkArea>();
            WorkAreaIncomingMovements = new ObservableCollection<WorkAreaMovement>();
            WorkAreaOutgoingMovements = new ObservableCollection<WorkAreaMovement>();
            WorkAreaIncomingMovementsCollectionView = CollectionViewSource.GetDefaultView(WorkAreaIncomingMovements);
            WorkAreaOutgoingMovementsCollectionView = CollectionViewSource.GetDefaultView(WorkAreaOutgoingMovements);
        }

        public WorkArea SelectedWorkArea
        {
            get => _selectedWorkArea;

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
                ReloadMovementCollection();
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;

            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                ReloadMovementCollection();
            }
        }

        public ObservableCollection<WorkArea> WorkAreas { get; }

        public ObservableCollection<WorkAreaMovement> WorkAreaIncomingMovements { get; }

        public ObservableCollection<WorkAreaMovement> WorkAreaOutgoingMovements { get; }

        public ICollectionView WorkAreaIncomingMovementsCollectionView { get; }
        public ICollectionView WorkAreaOutgoingMovementsCollectionView { get; }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadWorkAreas();

            SelectedDate = DateTime.Today;

            if (id.HasValue)
            {
                SelectedWorkArea = WorkAreas.Single(w => w.Id == id.Value);
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private async Task LoadWorkAreas()
        {
            var workAreas  = await _workAreaMovementRepository.GetAllWorkAreasFromBranchAsync(SessionInfo.Branch.Id);

            foreach (var workArea in workAreas)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreas.Add(workArea));
            }
        }

        private async Task ReloadMovementCollection()
        {
            if (SelectedWorkArea != null)
            {
                WorkAreaIncomingMovements.Clear();
                WorkAreaOutgoingMovements.Clear();

                var movements =
                    await _workAreaMovementRepository.GetAllIncomingWorkAreaMovementsOfWorkAreaInDateAsync(
                        SelectedWorkArea.Id, SelectedDate);
                foreach (var movement in movements)
                {
                    Application.Current.Dispatcher.Invoke(() => WorkAreaIncomingMovements.Add(movement));
                }

                movements =
                    await _workAreaMovementRepository.GetAllOutgoingWorkAreaMovementsOfWorkAreaInDateAsync(
                        SelectedWorkArea.Id, SelectedDate);
                foreach (var movement in movements)
                {
                    Application.Current.Dispatcher.Invoke(() => WorkAreaOutgoingMovements.Add(movement));
                }
            }
        }
    }
}
