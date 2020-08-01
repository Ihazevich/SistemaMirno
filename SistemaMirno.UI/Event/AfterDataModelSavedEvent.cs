using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Event
{
    public class AfterDataModelSavedEvent<T> : PubSubEvent<AfterDataModelSavedEventArgs<T>>
    {
    }

    public class AfterDataModelSavedEventArgs<T>
    {
        public T Model { get; set; }
    }
}
