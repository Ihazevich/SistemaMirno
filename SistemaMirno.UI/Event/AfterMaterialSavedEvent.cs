using Prism.Events;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Event
{
    public class AfterMaterialSavedEvent : PubSubEvent<AfterMaterialSavedEventArgs>
    {
    }

    public class AfterMaterialSavedEventArgs
    {
        public Material Material { get; set; }
    }
}