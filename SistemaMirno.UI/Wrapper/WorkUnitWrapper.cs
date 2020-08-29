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

        public int Id => GetValue<int>();

        public int ProductId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int MaterialId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }
        
        public int ColorId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int CurrentWorkAreaId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int RequisitionId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public DateTime CreationDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public double TotalWorkTime
        {
            get => GetValue<double>();
            set => SetValue(value);
        }

        public bool Delivered
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool Sold
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public int? LatestResponsibleId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public int? LatestSupervisorId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public string Details
        {
            get => GetValue<string>();
            set => SetValue(value);
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

            foreach (var error in base.ValidateProperty(propertyName))
            {
                if (error != null)
                {
                    yield return error;
                }
            }
        }
    }
}
