using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkAreaConnectionWrapper : ModelWrapper<WorkAreaConnection>
    {
        public WorkAreaConnectionWrapper()
            : base(new WorkAreaConnection())
        {
        }

        public WorkAreaConnectionWrapper(WorkAreaConnection model)
            : base(model)
        {
        }

        public int Id { get { return GetValue<int>(); } }

        public int OriginWorkAreaId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int DestinationWorkAreaId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }
    }
}
