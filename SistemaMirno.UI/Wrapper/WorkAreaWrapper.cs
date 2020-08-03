using SistemaMirno.Model;
using System.Collections.Generic;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// A class representing the wrapper for the Production Area class.
    /// </summary>
    public class WorkAreaWrapper : ModelWrapper<WorkArea>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="WorkArea"> to use as model.</param>
        public WorkAreaWrapper(WorkArea model)
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

        /// <summary>
        /// Gets or sets the Production Area order.
        /// </summary>
        public int? WorkAreaResponsibleRoleId
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the Production Area order.
        /// </summary>
        public int? WorkAreaSupervisorRoleId
        {
            get { return GetValue<int?>(); }
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