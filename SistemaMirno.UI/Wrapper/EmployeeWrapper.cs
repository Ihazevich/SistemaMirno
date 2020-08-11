using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// A class representing the wrapper for the Employee model.
    /// </summary>
    public class EmployeeWrapper : ModelWrapper<Employee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="Employee"> to use as model.</param>
        public EmployeeWrapper(Employee model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the Employee ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the Employee first name.
        /// </summary>
        public string FirstName
        {
            get => GetValue<string>();

            set
            {
                SetValue(value);
                OnPropertyChanged("FullName");
            }
        }

        /// <summary>
        /// Gets or sets the Employee last name.
        /// </summary>
        public string LastName
        {
            get => GetValue<string>();

            set
            {
                SetValue(value);
                OnPropertyChanged("FullName");
            }
        }

        /// <summary>
        /// Gets the employee full name.
        /// </summary>
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        /// <summary>
        /// Gets or sets the Employee document number.
        /// </summary>
        public int DocumentNumber
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the date the employee started working.
        /// </summary>
        public DateTime HiredDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the role id assigned to the employee.
        /// </summary>
        public int EmployeeRoleId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the collection of work orders of which the employee is/was responsible.
        /// </summary>
        public Collection<WorkOrder> ResponsibleWorkOrders
        {
            get { return GetValue<Collection<WorkOrder>>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the collection of work orders the employee is/was supervising.
        /// </summary>
        public Collection<WorkOrder> SupervisorWorkOrders
        {
            get { return GetValue<Collection<WorkOrder>>(); }
            set { SetValue(value); }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (FirstName.Length < 4)
                    {
                        yield return "El nombre es muy corto.";
                    }

                    break;

                case nameof(LastName):
                    if (FirstName.Length < 4)
                    {
                        yield return "El apellido es muy corto.";
                    }

                    break;
            }
        }
    }
}
