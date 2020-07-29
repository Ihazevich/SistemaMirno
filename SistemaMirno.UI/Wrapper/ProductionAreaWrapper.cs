using SistemaMirno.Model;
using System.Collections;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// Data wrapper for the Production Area model.
    /// </summary>
    public class ProductionAreaWrapper : WrapperBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionAreaWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="ProductionArea"/> class to use as a model.</param>
        public ProductionAreaWrapper(ProductionArea model)
        {
            Model = model;
        }

        /// <summary>
        /// Gets or sets the wrapper model.
        /// </summary>
        public ProductionArea Model { get; set; }

        /// <summary>
        /// Gets the Production Area ID.
        /// </summary>
        public int Id { get { return Model.Id; } }

        /// <summary>
        /// Gets or sets the Production Area name.
        /// </summary>
        public string Name
        {
            get
            {
                return Model.Name;
            }

            set
            {
                Model.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Production Area order.
        /// </summary>
        public int Order
        {
            get
            {
                return Model.Order;
            }

            set
            {
                Model.Order = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override IEnumerable GetErrors(string propertyName)
        {
            return base.GetErrors(propertyName);
        }
    }
}
