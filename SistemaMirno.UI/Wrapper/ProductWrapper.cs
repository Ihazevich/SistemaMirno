using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// A class representing the wrapper for the Material class.
    /// </summary>
    public class ProductWrapper : ModelWrapper<Product>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="Product"> to use as model.</param>
        public ProductWrapper(Product model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the Product ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the Product code.
        /// </summary>
        public string Code
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the Product name.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the Product ProductCategoryId.
        /// </summary>
        public int ProductCategoryId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the Product Price.
        /// </summary>
        public int Price
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the Product WholesalePrice.
        /// </summary>
        public int WholesalePrice
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the Product ProductionPrice.
        /// </summary>
        public int ProductionPrice
        {
            get { return GetValue<int>(); }
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
