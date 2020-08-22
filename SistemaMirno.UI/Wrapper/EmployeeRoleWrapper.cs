using System.Collections.Generic;
using System.Collections.ObjectModel;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class EmployeeRoleWrapper : ModelWrapper<Role>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRoleWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="Role"> to use as model.</param>
        public EmployeeRoleWrapper(Role model)
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

        public Collection<Employee> Employees
        {
            get { return GetValue<Collection<Employee>>(); }
            set { SetValue(value); }
        }

        public Collection<WorkArea> WorkAreasResponsibles
        {
            get { return GetValue<Collection<WorkArea>>(); }
            set { SetValue(value); }
        }

        public Collection<WorkArea> WorkAreasSupervisors
        {
            get { return GetValue<Collection<WorkArea>>(); }
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
