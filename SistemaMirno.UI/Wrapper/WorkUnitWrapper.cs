using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkUnitWrapper : ModelWrapper<WorkUnit>
    {
        public WorkUnitWrapper()
            : base(new WorkUnit())
        {
        }

        public WorkUnitWrapper(WorkUnit model)
            : base(model)
        {
        }

        public bool IsNew { get; set; } = false;

        public int Id { get { return GetValue<int>(); } }

        public int ProductId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int MaterialId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }
        
        public int ColorId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int CurrentWorkAreaId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int RequisitionId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public DateTime CreationDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public double TotalWorkTime
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public int LatestResponsibleId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int LatestSupervisorId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Details
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ProductId):
                    if (ProductId < 1)
                    {
                        yield return "Debe seleccionar un producto.";
                    }

                    break;

                case nameof(MaterialId):
                    if (MaterialId < 1)
                    {
                        yield return "Debe seleccionar un material.";
                    }

                    break;

                case nameof(ColorId):
                    if (ColorId < 1)
                    {
                        yield return "Debe seleccionar un lustre/color.";
                    }

                    break;
            }
        }
    }
}
