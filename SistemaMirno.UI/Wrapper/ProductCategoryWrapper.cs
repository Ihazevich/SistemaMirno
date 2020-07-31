using SistemaMirno.Model;
using System.Collections.Generic;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// A class representing the wrapper for the ProductCategory class.
    /// </summary>
    public class ProductCategoryWrapper : ModelWrapper<ProductCategory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="ProductCategory"> to use as model.</param>
        public ProductCategoryWrapper(ProductCategory model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the ProductCategory ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the ProductCategory name.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
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
            }
        }
    }
}