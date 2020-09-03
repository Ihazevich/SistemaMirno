// <copyright file="NavigationViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
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
            WorkAreas = new ObservableCollection<WorkArea>();

            ChangeViewCommand = new DelegateCommand<object>(ChangeView);

            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Subscribe(ChangeNavigation);
        }

        public ObservableCollection<WorkArea> WorkAreas { get; }

        public bool NavigationEnabled
        {
            get => _navigationEnabled;

            set
            {
                _navigationEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeViewCommand { get; }

        private void ChangeNavigation(bool arg)
        {
            NavigationEnabled = arg;
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                var workAreas = await _workAreaRepository.GetAllWorkAreasFromBranchAsync(id.Value);

                if (workAreas == null)
                {
                    return;
                }

                foreach (var workArea in workAreas)
                {
                    if (workArea.IsLast)
                    {
                        workArea.Position = 9999.ToString();
                    }
                    else if (workArea.IsFirst)
                    {
                        workArea.Position = 0.ToString();
                    }
                }

                workAreas.Sort((a, b) => string.Compare(a.Position, b.Position, StringComparison.Ordinal));

                foreach (var workArea in workAreas)
                {
                        Application.Current.Dispatcher.Invoke(() => WorkAreas.Add(workArea));
                }
            }
        }

        private void ChangeView(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var drawer = DrawerHost.CloseDrawerCommand;
            drawer.Execute(null, null);

            var workAreaId = int.Parse(obj.ToString());
            var workArea = WorkAreas.Single(w => w.Id == workAreaId);

            // If the work area name is the first or last, then redirect to the specialized views of those areas instead.
            if (workArea.IsFirst)
            {
                EventAggregator.GetEvent<ChangeViewEvent>()
                    .Publish(new ChangeViewEventArgs
                    {
                        ViewModel = nameof(RequisitionViewModel),
                    });
            }
            else if (workArea.IsLast)
            {
                EventAggregator.GetEvent<ChangeViewEvent>()
                    .Publish(new ChangeViewEventArgs
                    {
                        Id = SessionInfo.Branch.Id,
                        ViewModel = nameof(StockViewModel),
                    });
            }
            else
            {
                EventAggregator.GetEvent<ChangeViewEvent>()
                    .Publish(new ChangeViewEventArgs
                    {
                        Id = workArea.Id,
                        ViewModel = nameof(WorkUnitViewModel),
                    });
            }

            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(false);
        }
    }
}