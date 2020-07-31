using Prism.Events;
using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
