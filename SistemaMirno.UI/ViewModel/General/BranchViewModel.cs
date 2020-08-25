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
        private IBranchDetailViewModel _branchDetailViewModel;
        private Func<IBranchDetailViewModel> _branchDetailViewModelCreator;

        public BranchViewModel(
            Func<IBranchDetailViewModel> branchDetailViewModelCreator,
            Func<IBranchRepository> branchRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Sucursales", dialogCoordinator)
        {
            _branchDetailViewModelCreator = branchDetailViewModelCreator;
            _branchRepositoryCreator = branchRepositoryCreator;

            Branches = new ObservableCollection<BranchWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);

            EventAggregator.GetEvent<CloseDetailViewEvent<BranchDetailViewModel>>()
                .Subscribe(CloseDetailView, ThreadOption.UIThread);
        }

        protected override async void CloseDetailView()
        {
            base.CloseDetailView();
            BranchDetailViewModel = null;

            await LoadAsync();
        }

        public IBranchDetailViewModel BranchDetailViewModel
        {
            get
            {
                return _branchDetailViewModel;
            }

            private set
            {
                _branchDetailViewModel = value;
                OnPropertyChanged();
            }
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
                if (_selectedBranch != null)
                {
                    UpdateDetailViewModel(_selectedBranch.Id);
                }
            }
        }

        public ICommand CreateNewCommand { get; }

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

        private async Task UpdateDetailViewModel(int id)
        {
            if (BranchDetailViewModel != null && BranchDetailViewModel.HasChanges)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Guarde o descarte los cambios antes de salir",
                        Title = "Advertencia",
                    });
                return;
            }

            BranchDetailViewModel = _branchDetailViewModelCreator();
            await BranchDetailViewModel.LoadDetailAsync(id).ConfigureAwait(false);
        }

        private void OnCreateNewExecute()
        {
            if (BranchDetailViewModel != null && BranchDetailViewModel.HasChanges)
            {
                EventAggregator.GetEvent<ShowDialogEvent>()
                    .Publish(new ShowDialogEventArgs
                    {
                        Message = "Guarde o descarte los cambios antes de salir",
                        Title = "Advertencia",
                    });
                return;
            }

            BranchDetailViewModel = _branchDetailViewModelCreator();
            BranchDetailViewModel.IsNew = true;
            BranchDetailViewModel.LoadAsync();
        }
    }
}
