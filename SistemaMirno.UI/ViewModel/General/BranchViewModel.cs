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
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class BranchViewModel : ViewModelBase, IBranchViewModel
    {
        private IBranchRepository _branchRepository;
        private Func<IBranchRepository> _branchRepositoryCreator;
        private BranchWrapper _selectedBranch;

        public BranchViewModel(
            Func<IBranchRepository> branchRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Sucursales", dialogCoordinator)
        {
            _branchRepositoryCreator = branchRepositoryCreator;

            Branches = new ObservableCollection<BranchWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedBranch != null;
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedBranch.Id,
                    ViewModel = nameof(BranchDetailViewModel),
                });
        }

        public ObservableCollection<BranchWrapper> Branches { get; set; }

        public BranchWrapper SelectedBranch
        {
            get
            {
                return _selectedBranch;
            }

            set
            {
                OnPropertyChanged();
                _selectedBranch = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Branches.Clear();
            _branchRepository = _branchRepositoryCreator();

            var branches = await _branchRepository.GetAllAsync();

            foreach (var branch in branches)
            {
                Application.Current.Dispatcher.Invoke(() => Branches.Add(new BranchWrapper(branch)));
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
                    ViewModel = nameof(BranchDetailViewModel),
                });
        }
    }
}
