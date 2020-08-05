using System.Collections.Generic;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.Event
{
    public class NewMoveWorkOrderEvent : PubSubEvent<NewMoveWorkOrderEventArgs>
    {
    }

    public class NewMoveWorkOrderEventArgs
    {
        public ICollection<WorkUnitWrapper> WorkUnits { get; set; }

        public int WorkAreaId { get; set; }
    }
}
