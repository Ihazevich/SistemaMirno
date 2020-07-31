using Prism.Events;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Event
{
    public class AfterColorSavedEvent : PubSubEvent<AfterColorSavedEventArgs>
    {
    }

    public class AfterColorSavedEventArgs
    {
        public Color Color { get; set; }
    }
}