using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Wrapper
{
    public class EmployeeRoleWrapper : ModelWrapper<EmployeeRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRoleWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="EmployeeRole"> to use as model.</param>
        public EmployeeRoleWrapper(EmployeeRole model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the EmployeeRole ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the EmployeeRole name.
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
