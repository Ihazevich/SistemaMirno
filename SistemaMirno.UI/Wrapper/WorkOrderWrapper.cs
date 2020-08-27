using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkOrderWrapper : ModelWrapper<WorkOrder>
    {
        public WorkOrderWrapper()
            : base(new WorkOrder())
        {
        }

        public WorkOrderWrapper(WorkOrder model)
            : base(model)
        {
        }

        public int Id { get { return GetValue<int>(); } }

        public DateTime CreationDateTime
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public DateTime? FinishedDateTime
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public int OriginWorkAreaId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int DestinationWorkAreaId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int ResponsibleEmployeeId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int SupervisorEmployeeId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Observations
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ResponsibleEmployeeId):
                    if (ResponsibleEmployeeId < 1)
                    {
                        yield return "Debe seleccionar un responsable.";
                    }

                    break;

                case nameof(SupervisorEmployeeId):
                    if (SupervisorEmployeeId < 1)
                    {
                        yield return "Debe seleccionar un supervisor.";
                    }

                    break;
            }
        }
    }
}
