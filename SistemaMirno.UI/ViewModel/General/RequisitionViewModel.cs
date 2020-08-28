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
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class RequisitionViewModel : ViewModelBase, IRequisitionViewModel
    {
        private IRequisitionRepository _requisitionRepository;
        private Func<IRequisitionRepository> _requisitionRepositoryCreator;
        private RequisitionWrapper _selectedRequisition;

        public RequisitionViewModel(
            Func<IRequisitionRepository> requisitionRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Pedidos", dialogCoordinator)
        {
            _requisitionRepositoryCreator = requisitionRepositoryCreator;

            Requisitions = new ObservableCollection<RequisitionWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedRequisition.Id,
                    ViewModel = nameof(RequisitionDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedRequisition != null;
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionDetailViewModel),
                });
        }

        public ObservableCollection<RequisitionWrapper> Requisitions { get; set; }

        public RequisitionWrapper SelectedRequisition
        {
            get
            {
                return _selectedRequisition;
            }

            set
            {
                OnPropertyChanged();
                _selectedRequisition = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Requisitions.Clear();
            _requisitionRepository = _requisitionRepositoryCreator();

            var requisitions = await _requisitionRepository.GetAllAsync();

            foreach (var requisition in requisitions)
            {
                Application.Current.Dispatcher.Invoke(() => Requisitions.Add(new RequisitionWrapper(requisition)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }
    }
}
