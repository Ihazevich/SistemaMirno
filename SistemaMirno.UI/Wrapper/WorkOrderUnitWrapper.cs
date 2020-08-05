using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkOrderUnitWrapper : ModelWrapper<WorkOrderUnit>
    {
        public WorkOrderUnitWrapper(WorkOrderUnit model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the Work Order ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        public int WorkOrderId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public WorkOrder WorkOrder
        {
            get { return GetValue<WorkOrder>(); }
            set { SetValue(value); }
        }

        public int WorkUnitId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public WorkUnit WorkUnit
        {
            get { return GetValue<WorkUnit>(); }
            set { SetValue(value); }
        }
    }
}
