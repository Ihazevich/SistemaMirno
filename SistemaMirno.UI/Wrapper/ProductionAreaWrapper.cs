using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// Wrapper class for the Production Area model.
    /// </summary>
    public class ProductionAreaWrapper : ModelWrapper<ProductionArea>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionAreaWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="ProductionArea"> to use as model.</param>
        public ProductionAreaWrapper(ProductionArea model)
            : base(model)
        {

        }

        /// <summary>
        /// Gets the Production Area ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the Production Area name.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the Production Area order.
        /// </summary>
        public int Order
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
