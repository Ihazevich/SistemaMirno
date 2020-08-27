using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkOrderUnitWrapper : ModelWrapper<WorkOrderUnit>
    {
        public WorkOrderUnitWrapper()
            : base(new WorkOrderUnit())
        {
        }

        public WorkOrderUnitWrapper(WorkOrderUnit model)
            : base(model)
        {
        }

        public int Id
        {
            get { return GetValue<int>(); }
        }

        public int WorkOrderId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int WorkUnitId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public DateTime? FinisheDateTime
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            yield return null;
        }
    }
}
