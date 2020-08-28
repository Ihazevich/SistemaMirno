using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Event
{

    public class ShowDialogEventArgs
    {
        public string Message { get; set; }

        public string Title { get; set; }
    }
}
