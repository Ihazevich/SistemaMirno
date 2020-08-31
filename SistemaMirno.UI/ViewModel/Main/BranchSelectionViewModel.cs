using System.Collections.ObjectModel;
using System.Linq;
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

namespace SistemaMirno.UI.ViewModel.Main
{
    public class BranchSelectionViewModel : ViewModelBase, IBranchSelectionViewModel
    {
        private IBranchRepository _branchRepository;
        private BranchWrapper _selectedBranch;

        public BranchSelectionViewModel(
            IBranchRepository branchRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Seleccion de sucursal", dialogCoordinator)
        {
            _branchRepository = branchRepository;

            Branches = new ObservableCollection<BranchWrapper>();

            ChangeBranchCommand = new DelegateCommand<object>(OnSelectBranchExecute, OnSelectBranchCanExecute);
            CancelCommand = new DelegateCommand(OnCancelExecute);
            AddBranchCommand = new DelegateCommand(OnAddBranchExecute);

        }

        private void OnAddBranchExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    ViewModel = nameof(BranchDetailViewModel),
                });
        }

        public Visibility NewBranchButtonVisibility => SessionInfo.User != null && SessionInfo.User.Model.IsSystemAdmin
            ? Visibility.Visible
            : Visibility.Collapsed;

        private void OnCancelExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    ViewModel = nameof(LoginViewModel),
                });
        }

        private void OnSelectBranchExecute(object obj)
        {
            var branch = Branches.Single(b => b.Id == int.Parse(obj.ToString()));
            EventAggregator.GetEvent<BranchChangedEvent>()
                .Publish(new BranchChangedEventArgs
                {
                    BranchId = branch.Id,
                    Name = branch.Name,
                });
        }

        private bool OnSelectBranchCanExecute(object obj)
        {
            return SessionInfo.User.Model.IsSystemAdmin || SessionInfo.User.Model.Employee.Roles.Select(r => r.BranchId)
                .Contains(int.Parse(obj.ToString()));
        }

        public ObservableCollection<BranchWrapper> Branches { get; }

        public BranchWrapper SelectedBranch
        {
            get => _selectedBranch;

            set
            {
                _selectedBranch = value;
                OnPropertyChanged();
                ((DelegateCommand<object>)ChangeBranchCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand ChangeBranchCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand AddBranchCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Branches.Clear();

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
    }
}
