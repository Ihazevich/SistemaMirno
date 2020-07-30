using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// A class representing the wrapper for the AreaConnection class.
    /// </summary>
    public class AreaConnectionWrapper : ModelWrapper<AreaConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaConnectionWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="AreaConnection"> to use as model.</param>
        public AreaConnectionWrapper(AreaConnection model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the AreaConnection ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the AreaConnection FromAreaId.
        /// </summary>
        public string FromAreaId
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the AreaConnection ToAreaId.
        /// </summary>
        public string ToAreaId
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            yield return null;
        }
    }
}
