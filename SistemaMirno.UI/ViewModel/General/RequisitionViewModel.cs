// <copyright file="RequisitionViewModel.cs" company="HazeLabs">
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
    public class RequisitionViewModel : ViewModelBase
    {
        private readonly IRequisitionRepository _requisitionRepository;
        private RequisitionWrapper _selectedRequisition;

        public RequisitionViewModel(
            IRequisitionRepository requisitionRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Pedidos", dialogCoordinator)
        {
            _requisitionRepository = requisitionRepository;

            Requisitions = new ObservableCollection<RequisitionWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
            SaveChangesCommand = new DelegateCommand(OnSaveChangesExecute);
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public ICommand SaveChangesCommand { get; }

        public ObservableCollection<RequisitionWrapper> Requisitions { get; }

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

        public override async Task LoadAsync(int? id = null)
        {
            Requisitions.Clear();

            var requisitions = await _requisitionRepository.GetAllOpenRequisitionsAsync();

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

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedRequisition != null;
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

        private async void OnSaveChangesExecute()
        {
            foreach (var requisition in Requisitions)
            {
                await _requisitionRepository.SaveAsync(requisition.Model);
            }

            await LoadAsync().ConfigureAwait(false);
        }
    }
}
