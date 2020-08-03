using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkOrderWrapper : ModelWrapper<WorkOrder>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="WorkArea"> to use as model.</param>
        public WorkOrderWrapper(WorkOrder model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the Work Order ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the Work Order StartTime.
        /// </summary>
        public DateTime StartTime
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the Work Order FinishTime.
        /// </summary>
        public DateTime FinishTime
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the ProductionAreaId.
        /// </summary>
        public int WorkAreaId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public WorkArea WorkArea
        {
            get { return GetValue<WorkArea>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the ResponsibleEmployeeId.
        /// </summary>
        public int ResponsibleEmployeeId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public Employee ResponsibleEmployee
        {
            get { return GetValue<Employee>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the ResponsibleEmployeeId.
        /// </summary>
        public int SupervisorEmployeeID
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public Employee SupervisorEmployee
        {
            get { return GetValue<Employee>(); }
            set { SetValue(value); }
        }

        public Collection<WorkUnit> WorkUnits
        {
            get { return GetValue<Collection<WorkUnit>>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(StartTime):
                    if (StartTime > DateTime.Today)
                    {
                        yield return "La orden no puede empezar en el futuro. >:C";
                    }

                    break;
            }
        }
    }
}
