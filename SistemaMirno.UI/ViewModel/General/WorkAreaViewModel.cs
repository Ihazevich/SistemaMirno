// <copyright file="WorkAreaViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class WorkAreaViewModel : ViewModelBase
    {
        private readonly IWorkAreaRepository _workAreaRepository;
        private WorkAreaWrapper _selectedWorkArea;

        public WorkAreaViewModel(
            IWorkAreaRepository workAreaRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Areas de Trabajo", dialogCoordinator)
        {
            _workAreaRepository = workAreaRepository;

            WorkAreas = new ObservableCollection<WorkAreaWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public WorkAreaWrapper SelectedWorkArea
        {
            get => _selectedWorkArea;

            set
            {
                OnPropertyChanged();
                _selectedWorkArea = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; }

        public override async Task LoadAsync(int? id = null)
        {
            WorkAreas.Clear();

            var workAreas = await _workAreaRepository.GetAllAsync();

            foreach (var workArea in workAreas)
            {
                Application.Current.Dispatcher.Invoke(() => WorkAreas.Add(new WorkAreaWrapper(workArea)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(WorkAreaDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedWorkArea != null;
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedWorkArea.Id,
                    ViewModel = nameof(WorkAreaDetailViewModel),
                });
        }
    }
}
