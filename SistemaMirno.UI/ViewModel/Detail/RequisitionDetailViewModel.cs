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
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class RequisitionDetailViewModel : DetailViewModelBase, IRequisitionDetailViewModel
    {
        private IRequisitionRepository _requisitionRepository;
        private RequisitionWrapper _requisition;
        private WorkUnitWrapper _newWorkUnit;
        private bool _hasTargetDate;
        private Visibility _newWorkUnitGridVisibility;
        private Visibility _existingWorkUnitGridVisibility;
        private string _quantity;

        public RequisitionDetailViewModel(
            IRequisitionRepository requisitionRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Pedido", dialogCoordinator)
        {
            _requisitionRepository = requisitionRepository;

            Clients = new ObservableCollection<ClientWrapper>();
            Products = new ObservableCollection<ProductWrapper>();
            Materials = new ObservableCollection<MaterialWrapper>();
            Colors = new ObservableCollection<ColorWrapper>();

            AddWorkUnitCommand = new DelegateCommand<object>(OnAddWorkUnitExecute, OnAddWorkUnitCanExecute);
        }

        public string Quantity
        {
            get => _quantity;

            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ClientWrapper> Clients { get; }

        public ObservableCollection<ProductWrapper> Products { get; }

        public ObservableCollection<MaterialWrapper> Materials { get; }

        public ObservableCollection<ColorWrapper> Colors { get; }

        private bool OnAddWorkUnitCanExecute(object arg)
        {
            throw new NotImplementedException();
        }

        private void OnAddWorkUnitExecute(object obj)
        {
            if (obj.ToString() == "New")
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NewWorkUnitViewVisibility = Visibility.Visible;
                    ExistingWorkUnitViewVisibility = Visibility.Collapsed;
                });
            }

            if (obj.ToString() == "Exiting")
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NewWorkUnitViewVisibility = Visibility.Collapsed;
                    ExistingWorkUnitViewVisibility = Visibility.Visible;
                });
            }
        }

        public Visibility NewWorkUnitViewVisibility
        {
            get => _newWorkUnitGridVisibility;

            set
            {
                _newWorkUnitGridVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility ExistingWorkUnitViewVisibility
        {
            get => _existingWorkUnitGridVisibility;

            set
            {
                _existingWorkUnitGridVisibility = value;
                OnPropertyChanged();
            }
        }

        public RequisitionWrapper Requisition
        {
            get => _requisition;

            set
            {
                _requisition = value;
                OnPropertyChanged();
            }
        }

        public WorkUnitWrapper NewWorkUnit
        {
            get => _newWorkUnit;

            set
            {
                _newWorkUnit = value;
                OnPropertyChanged();
            }
        }

        public bool HasTargetDate
        {
            get => _hasTargetDate;

            set
            {
                _hasTargetDate = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddWorkUnitCommand { get; }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _requisitionRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Requisition = new RequisitionWrapper(model);
                Requisition.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            if (IsNew)
            {
                await _requisitionRepository.AddAsync(Requisition.Model);
            }
            else
            {
                await _requisitionRepository.SaveAsync(Requisition.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Requisition);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            await _requisitionRepository.DeleteAsync(Requisition.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RequisitionViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _requisitionRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Requisition.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            await LoadClients();
            await LoadMaterials();
            await LoadColors();
            await LoadProducts();

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;
                HasTargetDate = false;
                NewWorkUnit = new WorkUnitWrapper();

                Requisition = new RequisitionWrapper();
                Requisition.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                Requisition.RequestedDate = DateTime.Now;
                Requisition.Fulfilled = false;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadProducts()
        {
            throw new NotImplementedException();
        }

        private async Task LoadColors()
        {
            throw new NotImplementedException();
        }

        private async Task LoadMaterials()
        {
            throw new NotImplementedException();
        }

        private async Task LoadClients()
        {
            var clients = await _requisitionRepository.GetAllClientsAsync();

            foreach (var client in clients)
            {
                Application.Current.Dispatcher.Invoke(() => Clients.Add(new ClientWrapper(client)));
            }
        }
    }
}
