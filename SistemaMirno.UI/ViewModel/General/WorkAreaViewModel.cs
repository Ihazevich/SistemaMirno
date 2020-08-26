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
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class WorkAreaViewModel : ViewModelBase, IWorkAreaViewModel
    {
        private readonly Func<IWorkAreaRepository> _workAreaRepositoryCreator;
        private IWorkAreaRepository _workAreaRepository;
        private WorkAreaWrapper _selectedWorkArea;

        public WorkAreaViewModel(
            Func<IWorkAreaRepository> workAreaRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Areas de Trabajo", dialogCoordinator)
        {
            _workAreaRepositoryCreator = workAreaRepositoryCreator;

            WorkAreas = new ObservableCollection<WorkAreaWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
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

        private bool OnOpenDetailCanExecute()
        {
            return SelectedWorkArea != null;
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

        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; set; }

        public WorkAreaWrapper SelectedWorkArea
        {
            get =>_selectedWorkArea;

            set
            {
                OnPropertyChanged();
                _selectedWorkArea = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            WorkAreas.Clear();
            _workAreaRepository = _workAreaRepositoryCreator();

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
    }
}
