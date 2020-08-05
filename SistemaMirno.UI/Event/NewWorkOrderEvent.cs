using System.Collections.Generic;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.Event
{
    public class NewWorkOrderEvent : PubSubEvent<NewWorkOrderEventArgs>
    {
    }

    public class NewWorkOrderEventArgs
    {
        public ICollection<WorkUnitWrapper> WorkUnits { get; set; }

        public int OriginWorkAreaId { get; set; }

        public int DestinationWorkAreaId { get; set; }
    }
}
