using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Wrapper
{

    public class RequisitionWrapper : ModelWrapper<Requisition>
    {
        public RequisitionWrapper(Requisition model)
            : base(model)
        {
        }

        public int Id { get { return GetValue<int>(); } }

        public int ClientId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public virtual Client Client
        {
            get { return GetValue<Client>(); }
            set { SetValue(value); }
        }

        public DateTime RequestDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public DateTime FullfilledDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public virtual ICollection<WorkUnit> WorkUnits
        {
            get { return GetValue<ICollection<WorkUnit>>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}
