// <copyright file="RequisitionDetailViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class RequisitionDetailViewModel : DetailViewModelBase, IRequisitionDetailViewModel
    {
        private IRequisitionRepository _requisitionRepository;
        private RequisitionWrapper _requisition;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequisitionDetailViewModel"/> class.
        /// </summary>
        /// <param name="requisitionRepository">The data repository.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public RequisitionDetailViewModel(
            IRequisitionRepository requisitionRepository,
            IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _requisitionRepository = requisitionRepository;
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public RequisitionWrapper Requisition
        {
            get
            {
                return _requisition;
            }

            set
            {
                _requisition = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadAsync(int? requisitionId)
        {
            var requisition = new Requisition();
            if (requisitionId.HasValue)
            {
                requisition = await _requisitionRepository.GetByIdAsync(requisitionId.Value);
            }

            Requisition = new RequisitionWrapper(requisition);
        }
    }
}
