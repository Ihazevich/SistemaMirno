using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SistemaMirno.Model
{
    public class WorkOrder
    {
        public WorkOrder()
        {
            WorkUnits = new Collection<WorkUnit>();
        }

        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public int ProductionAreaId { get; set; }

        public ProductionArea ProductionArea { get; set; }

        public int ResponsibleId { get; set; }

        public Responsible Responsible { get; set; }

        public int SupervisorID { get; set; }

        public Supervisor Supervisor { get; set; }

        public Collection<WorkUnit> WorkUnits { get; set; }
    }
}