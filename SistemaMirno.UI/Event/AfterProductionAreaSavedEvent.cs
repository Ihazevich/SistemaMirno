using Prism.Events;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Event
{
    /// <summary>
    /// Class representing the Update View event for the Production Area detail view.
    /// </summary>
    public class AfterProductionAreaSavedEvent : PubSubEvent<AfterProductionAreaSavedEventArgs>
    {
    }

    public class AfterProductionAreaSavedEventArgs
    {
        public ProductionArea ProductionArea { get; set; }
    }
}
