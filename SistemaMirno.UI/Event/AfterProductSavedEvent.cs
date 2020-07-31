using Prism.Events;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Event
{
    public class AfterProductSavedEvent : PubSubEvent<AfterProductSavedEventArgs>
    {
    }

    public class AfterProductSavedEventArgs
    {
        public Product Product { get; set; }
    }
}
