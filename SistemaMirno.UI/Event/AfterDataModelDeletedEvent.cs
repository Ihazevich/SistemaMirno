using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Event
{
    public class AfterDataModelDeletedEvent<T> :PubSubEvent<AfterDataModelDeletedEventArgs<T>>
    {
    }

    public class AfterDataModelDeletedEventArgs<T>
    {
        public T Model { get; set; }
    }
}
