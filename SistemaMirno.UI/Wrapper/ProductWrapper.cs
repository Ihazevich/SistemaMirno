using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class ProductWrapper : ModelWrapper<Product>
    {
        public ProductWrapper()
            : base(new Product())
        {
        }

        public ProductWrapper(Product model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public string Code
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int ProductCategoryId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public long ProductionValue
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long RetailPrice
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public long WholesalerPrice
        {
            get => GetValue<long>();
            set => SetValue(value);
        }

        public bool IsCustom
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string SketchupFile
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string TemplateFile
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name):
                    if (Name.Length < 4)
                    {
                        yield return "El nombre es muy corto.";
                    }

                    break;

                case nameof(Code):
                    if (Code.Length < 4)
                    {
                        yield return "Codigo debe tener al menos 4 caracteres.";
                    }

                    break;

                case nameof(ProductCategoryId):
                    if (ProductCategoryId < 1)
                    {
                        yield return "Debe seleccionar una categoria.";
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
