using Prism.Events;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Event
{
    public class AfterProductCategorySavedEvent : PubSubEvent<AfterProductCategorySavedEventArgs>
    {
    }

    public class AfterProductCategorySavedEventArgs
    {
        public ProductCategory ProductCategory { get; set; }
    }
}