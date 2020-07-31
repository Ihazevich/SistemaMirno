using SistemaMirno.Model;
using System.Collections.Generic;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// A class representing the wrapper for the Material class.
    /// </summary>
    public class MaterialWrapper : ModelWrapper<Material>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="Material"> to use as model.</param>
        public MaterialWrapper(Material model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the Material ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the Material name.
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