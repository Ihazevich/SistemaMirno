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

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="userDetailViewModelCreator">A function to create detailviewmodel instances.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> instance representing the event aggregator.</param>
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

        /// <summary>
        /// Gets the Branch detail view model.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the collection of Users.
        /// </summary>
        public ObservableCollection<BranchWrapper> Branches { get; set; }

        /// <summary>
        /// Gets or sets the selected Branch.
        /// </summary>
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

        /// <summary>
        /// Gets the create new user command.
        /// </summary>
        public ICommand CreateNewCommand { get; }

        /// <summary>
        /// Loads the view model asynchronously from the data service.
        /// </summary>
        /// <returns>An instance of the <see cref="Task"/> class where the loading happens.</returns>
        public override async Task LoadAsync()
        {
            Branches.Clear();
            _branchRepository = _branchRepositoryCreator();

            var branches = await _branchRepository.GetAllAsync();

            ProgressVisibility = Visibility.Collapsed;
            ViewVisibility = Visibility.Visible;

            foreach (var branch in branches)
            {
                Branches.Add(new BranchWrapper(branch));
            }
        }

        private async void UpdateDetailViewModel(int id)
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
            await BranchDetailViewModel.LoadDetailAsync(id);
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
