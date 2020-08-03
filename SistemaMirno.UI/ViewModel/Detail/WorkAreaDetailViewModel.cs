﻿using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel.Detail
{
    /// <summary>
    /// Class representing the detailed view model of a single Production Area.
    /// </summary>
    public class WorkAreaDetailViewModel : DetailViewModelBase, IWorkAreaDetailViewModel
    {
        private IWorkAreaRepository _productionAreaRepository;
        private IEmployeeRoleRepository _employeeRoleRepository;
        private IEventAggregator _eventAggregator;
        private WorkAreaWrapper _productionArea;
        private bool _hasChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaDetailViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public WorkAreaDetailViewModel(
            IWorkAreaRepository productionAreaRepository,
            IEmployeeRoleRepository employeeRoleRepository,
            IEventAggregator eventAggregator)
        {
            _productionAreaRepository = productionAreaRepository;
            _employeeRoleRepository = employeeRoleRepository;
            _eventAggregator = eventAggregator;

            EmployeeRoles = new ObservableCollection<EmployeeRoleWrapper>();
        }

        public ObservableCollection<EmployeeRoleWrapper> EmployeeRoles { get; }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public WorkAreaWrapper ProductionArea
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

        /// <summary>
        /// Gets or sets a value indicating whether the database context has changes.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return _hasChanges;
            }

            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        /// <inheritdoc/>
        public async Task LoadAsync(int? productionAreaId)
        {
            var productionArea = productionAreaId.HasValue
                ? await _productionAreaRepository.GetByIdAsync(productionAreaId.Value)
                : CreateNewProductionArea();

            ProductionArea = new WorkAreaWrapper(productionArea);
            ProductionArea.PropertyChanged += ProductionArea_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (productionArea.Id == 0)
            {
                // This triggers the validation.
                ProductionArea.Name = string.Empty;
            }

            await LoadEmployeeRolesAsync();
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

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _productionAreaRepository.SaveAsync();
            //HasChanges = _productionAreaRepository.HasChanges();
            HasChanges = false;
            _eventAggregator.GetEvent<AfterDataModelSavedEvent<WorkArea>>()
                .Publish(new AfterDataModelSavedEventArgs<WorkArea> { Model = ProductionArea.Model });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return ProductionArea != null && !ProductionArea.HasErrors && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            _productionAreaRepository.Remove(ProductionArea.Model);
            await _productionAreaRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterDataModelDeletedEvent<WorkArea>>()
                .Publish(new AfterDataModelDeletedEventArgs<WorkArea> { Model = ProductionArea.Model });
        }

        private void ProductionArea_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
            if (!HasChanges)
            {
                HasChanges = _productionAreaRepository.HasChanges();
            }

            if (e.PropertyName == nameof(ProductionArea.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private WorkArea CreateNewProductionArea()
        {
            var productionArea = new WorkArea();
            _productionAreaRepository.Add(productionArea);
            return productionArea;
        }
    }
}