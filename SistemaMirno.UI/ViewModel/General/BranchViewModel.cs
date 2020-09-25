// <copyright file="BranchViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
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
    /// <summary>
    /// Represents the <see cref="Model.Branch"/> View Model.
    /// </summary>
    public class BranchViewModel : ViewModelBase
    {
        private readonly IBranchRepository _branchRepository;
        private BranchWrapper _selectedBranch;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchViewModel"/> class.
        /// </summary>
        /// <param name="branchRepository">The repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="dialogCoordinator">The dialog coordinator.</param>
        public BranchViewModel(
            IBranchRepository branchRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Sucursales", dialogCoordinator)
        {
            _branchRepository = branchRepository;

            Branches = new ObservableCollection<BranchWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        public ObservableCollection<BranchWrapper> Branches { get; }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

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

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(BranchDetailViewModel),
                });
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
    }
}
