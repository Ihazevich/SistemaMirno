using Prism.Events;

namespace SistemaMirno.UI.Event
{
    public class ChangeViewEvent : PubSubEvent<ChangeViewEventArgs>
    {
    }

    public class ChangeViewEventArgs
    {
        public string ViewModel { get; set; }
        public int Id { get; set; }
    }
}