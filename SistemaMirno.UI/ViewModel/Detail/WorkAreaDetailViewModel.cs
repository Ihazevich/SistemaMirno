using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel.Detail
{
    /// <summary>
    /// Class representing the detailed view model of a single Production Area.
    /// </summary>
    public class WorkAreaDetailViewModel : DetailViewModelBase, IWorkAreaDetailViewModel
    {
        private IEmployeeRoleRepository _employeeRoleRepository;
        private WorkAreaWrapper _productionArea;
        private IWorkAreaRepository _productionAreaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaDetailViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public WorkAreaDetailViewModel(
            IWorkAreaRepository productionAreaRepository,
            IEmployeeRoleRepository employeeRoleRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _productionAreaRepository = productionAreaRepository;
            _employeeRoleRepository = employeeRoleRepository;

            EmployeeRoles = new ObservableCollection<EmployeeRoleWrapper>();
            EditAreaConnectionsCommand = new DelegateCommand(OnOpenAreaConnectionViewExecute);
        }

        public ICommand EditAreaConnectionsCommand { get; }

        public ObservableCollection<EmployeeRoleWrapper> EmployeeRoles { get; }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public WorkAreaWrapper WorkArea
        {
            get
            {
                return _productionArea;
            }

            set
            {
                _productionArea = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? productionAreaId)
        {
            var productionArea = productionAreaId.HasValue
                ? await _productionAreaRepository.GetByIdAsync(productionAreaId.Value)
                : CreateNewProductionArea();

            WorkArea = new WorkAreaWrapper(productionArea);
            WorkArea.PropertyChanged += ProductionArea_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (productionArea.Id == 0)
            {
                // This triggers the validation.
                WorkArea.Name = string.Empty;
            }

            await LoadEmployeeRolesAsync();
        }

        protected override async void OnDeleteExecute()
        {
            _productionAreaRepository.Remove(WorkArea.Model);
            await _productionAreaRepository.SaveAsync();
            RaiseDataModelDeletedEvent(WorkArea.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return WorkArea != null && !WorkArea.HasErrors && HasChanges;
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _productionAreaRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(WorkArea.Model);
        }

        private WorkArea CreateNewProductionArea()
        {
            var productionArea = new WorkArea();
            _productionAreaRepository.Add(productionArea);
            return productionArea;
        }

        private async Task LoadEmployeeRolesAsync()
        {
            var roles = await _employeeRoleRepository.GetAllAsync();
            EmployeeRoles.Clear();
            foreach (var role in roles)
            {
                EmployeeRoles.Add(new EmployeeRoleWrapper(role));
            }
        }

        private void OnOpenAreaConnectionViewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs { ViewModel = nameof(AreaConnectionViewModel), Id = WorkArea.Id });
        }

        private void ProductionArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
            if (!HasChanges)
            {
                HasChanges = _productionAreaRepository.HasChanges();
            }

            if (e.PropertyName == nameof(WorkArea.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }
    }
}