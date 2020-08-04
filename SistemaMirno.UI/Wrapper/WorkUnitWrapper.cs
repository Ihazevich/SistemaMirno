using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkUnitWrapper : ModelWrapper<WorkUnit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnitWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="WorkUnit"> to use as model.</param>
        public WorkUnitWrapper(WorkUnit model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the Color ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }


        public int ProductId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public virtual Product Product
        {
            get { return GetValue<Product>(); }
            set { SetValue(value); }
        }

        public int MaterialId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public virtual Material Material
        {
            get { return GetValue<Material>(); }
            set { SetValue(value); }
        }

        public int ColorId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public virtual Color Color
        {
            get { return GetValue<Color>(); }
            set { SetValue(value); }
        }

        public int WorkOrderId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public virtual WorkOrder WorkOrder
        {
            get { return GetValue<WorkOrder>(); }
            set { SetValue(value); }
        }

        public int ProductionAreaId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public virtual WorkArea ProductionArea
        {
            get { return GetValue<WorkArea>(); }
            set { SetValue(value); }
        }

        public int Quantity { get; set; }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ProductId):
                    if (ProductId < 1)
                    {
                        yield return "Debe seleccionar un producto";
                    }

                    break;
                case nameof(MaterialId):
                    if (MaterialId < 1)
                    {
                        yield return "Debe seleccionar un material";
                    }

                    break;
                case nameof(ColorId):
                    if (ColorId < 1)
                    {
                        yield return "Debe seleccionar un color/lustre";
                    }

                    break;
            }
        }
    }
}
