﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class AreaConnectionDetailViewModel : DetailViewModelBase, IAreaConnectionDetailViewModel
    {
        private AreaConnectionWrapper _areaConnection;
        private IAreaConnectionRepository _areaConnectionRepository;
        private IWorkAreaRepository _workAreaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeDetailViewModel"/> class.
        /// </summary>
        /// <param name="productRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public AreaConnectionDetailViewModel(
            IAreaConnectionRepository areaConnectionRepository,
            IWorkAreaRepository workAreaRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _areaConnectionRepository = areaConnectionRepository;
            _workAreaRepository = workAreaRepository;

            EventAggregator.GetEvent<AfterDataModelSavedEvent<WorkArea>>()
                .Subscribe(AfterWorkAreaSaved);
            EventAggregator.GetEvent<AfterDataModelDeletedEvent<WorkArea>>()
                .Subscribe(AfterWorkAreaDeleted);

            WorkAreas = new ObservableCollection<WorkAreaWrapper>();
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public AreaConnectionWrapper AreaConnection
        {
            get
            {
                return _areaConnection;
            }

            set
            {
                _areaConnection = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<WorkAreaWrapper> WorkAreas { get; }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? areaConnectionId)
        {
            var areaConnection = areaConnectionId.HasValue
                ? await _areaConnectionRepository.GetByIdAsync(areaConnectionId.Value)
                : CreateNewAreaConnection();

            AreaConnection = new AreaConnectionWrapper(areaConnection);
            AreaConnection.PropertyChanged += AreaConnection_PropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            await LoadWorkAreasAsync();
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            _areaConnectionRepository.Remove(AreaConnection.Model);
            await _areaConnectionRepository.SaveAsync();
            RaiseDataModelDeletedEvent(AreaConnection.Model);
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return AreaConnection != null && !AreaConnection.HasErrors && HasChanges;
        }

        /// <inheritdoc/>
        protected override void OnSaveExecute()
        {
            _areaConnectionRepository.SaveAsync();
            HasChanges = false;
            RaiseDataModelSavedEvent(AreaConnection.Model);
        }

        private void AfterWorkAreaDeleted(AfterDataModelDeletedEventArgs<WorkArea> args)
        {
            var item = WorkAreas.SingleOrDefault(r => r.Id == args.Model.Id);

            if (item != null)
            {
                WorkAreas.Remove(item);
            }
        }

        private void AfterWorkAreaSaved(AfterDataModelSavedEventArgs<WorkArea> args)
        {
            var item = WorkAreas.SingleOrDefault(r => r.Id == args.Model.Id);

            if (item == null)
            {
                WorkAreas.Add(new WorkAreaWrapper(args.Model));
            }
            else
            {
                item.Name = args.Model.Name;
            }
        }

        private void AreaConnection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
            if (!HasChanges)
            {
                HasChanges = _areaConnectionRepository.HasChanges();
            }

            if (e.PropertyName == nameof(AreaConnection.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private AreaConnection CreateNewAreaConnection()
        {
            var areaConnection = new AreaConnection();
            _areaConnectionRepository.Add(areaConnection);
            return areaConnection;
        }

        private async Task LoadWorkAreasAsync()
        {
            var areas = await _workAreaRepository.GetAllAsync();
            WorkAreas.Clear();
            foreach (var area in areas)
            {
                WorkAreas.Add(new WorkAreaWrapper(area));
            }
        }
    }
}