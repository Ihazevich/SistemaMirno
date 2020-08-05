using System.Collections.Generic;
using Prism.Events;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Event
{
    public class NewMoveWorkOrderEvent : PubSubEvent<NewMoveWorkOrderEventArgs>
    {
    }

    public class NewMoveWorkOrderEventArgs
    {
        ICollection<WorkUnit> WorkUnits { get; set; }

        int WorkAreaId { get; set; }
    }
}
