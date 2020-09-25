// <copyright file="WorkAreaMovementViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

namespace SistemaMirno.UI.ViewModel.General
{
    public class WorkAreaMovementViewModel : ViewModelBase
    {
        private readonly IWorkAreaMovementRepository _workAreaMovementRepository;
        private DateTime _selectedDate;
        private WorkArea _selectedWorkArea;

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

        public DateTime SelectedDate
        {
            get => _selectedDate;

            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                ReloadMovementCollection().ConfigureAwait(false);
            }
        }

        public WorkArea SelectedWorkArea
        {
            get => _selectedWorkArea;

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
                ReloadMovementCollection().ConfigureAwait(false);
            }
        }

        public ObservableCollection<WorkAreaMovement> WorkAreaIncomingMovements { get; }

        public ICollectionView WorkAreaIncomingMovementsCollectionView { get; }

        public ObservableCollection<WorkAreaMovement> WorkAreaOutgoingMovements { get; }

        public ICollectionView WorkAreaOutgoingMovementsCollectionView { get; }

        public ObservableCollection<WorkArea> WorkAreas { get; }

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
            var workAreas = await _workAreaMovementRepository.GetAllWorkAreasFromBranchAsync(SessionInfo.Branch.Id);

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
