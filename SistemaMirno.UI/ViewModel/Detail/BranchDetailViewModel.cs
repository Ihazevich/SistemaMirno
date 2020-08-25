using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class BranchDetailViewModel : DetailViewModelBase, IBranchDetailViewModel
    {
        private IBranchRepository _branchRepository;
        private BranchWrapper _branch;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchDetailViewModel"/> class.
        /// </summary>
        /// <param name="branchRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public BranchDetailViewModel(
            IBranchRepository branchRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Sucursal", dialogCoordinator)
        {
            _branchRepository = branchRepository;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public BranchWrapper Branch
        {
            get => _branch;

            set
            {
                _branch = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _branchRepository.GetByIdAsync(id);

            Branch = new BranchWrapper(model);
            Branch.PropertyChanged += Model_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            if (IsNew)
            {
                Branch.Cash = 0;
                _branchRepository.AddAsync(Branch.Model);
            }
            else
            {
                _branchRepository.SaveAsync(Branch.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<CloseDetailViewEvent<UserDetailViewModel>>()
                .Publish();
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return Branch != null && !Branch.HasErrors && HasChanges;
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            await _branchRepository.DeleteAsync(Branch.Model);
            EventAggregator.GetEvent<CloseDetailViewEvent<UserDetailViewModel>>()
                .Publish();
        }

        protected override void OnCancelExecute()
        {
            EventAggregator.GetEvent<CloseDetailViewEvent<UserDetailViewModel>>()
                .Publish();
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _branchRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Branch.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override Task LoadAsync()
        {
            Branch = new BranchWrapper();
            Branch.PropertyChanged += Model_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            Branch.Name = string.Empty;
            Branch.Address = string.Empty;
            Branch.City = string.Empty;
            Branch.Department = string.Empty;
            return null;
        }
    }
}
