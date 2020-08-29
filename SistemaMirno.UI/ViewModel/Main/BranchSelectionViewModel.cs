using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.View.Detail;
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

            SelectBranchCommand = new DelegateCommand(OnSelectBranchExecute, OnSelectBranchCanExecute);
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

        private void OnSelectBranchExecute()
        {
            EventAggregator.GetEvent<BranchChangedEvent>()
                .Publish(new BranchChangedEventArgs
                {
                    BranchId = SelectedBranch.Id,
                    Name = SelectedBranch.Name,
                });
        }

        private bool OnSelectBranchCanExecute()
        {
            return SelectedBranch != null;
        }

        public ObservableCollection<BranchWrapper> Branches { get; }

        public BranchWrapper SelectedBranch
        {
            get => _selectedBranch;

            set
            {
                _selectedBranch = value;
                OnPropertyChanged();
                ((DelegateCommand)SelectBranchCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand SelectBranchCommand { get; }

        public ICommand CancelCommand { get; }

        public ICommand AddBranchCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Branches.Clear();

            var branches = await _branchRepository.GetAllAsync();

            foreach (var branch in branches.Where(branch => SessionInfo.User.Model.IsSystemAdmin || SessionInfo.User.Model.Employee.Roles.Select(r => r.BranchId).Contains(branch.Id)))
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
