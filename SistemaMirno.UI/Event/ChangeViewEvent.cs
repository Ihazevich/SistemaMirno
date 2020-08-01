using Prism.Events;
using SistemaMirno.UI.ViewModel;
using System;

namespace SistemaMirno.UI.Event
{
    public class ChangeViewEvent : PubSubEvent<IViewModelBase>
    {
    }
}