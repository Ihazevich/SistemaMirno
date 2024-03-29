﻿using SistemaMirno.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkAreaWrapper : ModelWrapper<WorkArea>
    {
        public WorkAreaWrapper()
            : base(new WorkArea())
        {
        }

        public WorkAreaWrapper(WorkArea model)
            : base(model)
        {
        }

        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the User name.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the User password.
        /// </summary>
        public string Position
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public int BranchId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int ResponsibleRoleId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int SupervisorRoleId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public bool ReportsInProcess
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IsLast
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IsFirst
        {
            get { return GetValue<bool>(); }
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

                case nameof(Position):
                    if (int.TryParse(Position, out int position))
                    {
                        if (position < 0)
                        {
                            yield return "Posición no puede ser negativa.";
                        }
                    }
                    else
                    {
                        yield return "Posición debe ser un numero.";
                    }

                    break;

                case nameof(BranchId):
                    if (BranchId < 1)
                    {
                        yield return "Debe seleccionar una sucursal.";
                    }

                    break;


                case nameof(ResponsibleRoleId):
                    if (ResponsibleRoleId < 1)
                    {
                        yield return "Debe seleccionar un rol para los responsables.";
                    }

                    break;

                case nameof(SupervisorRoleId):
                    if (SupervisorRoleId < 1)
                    {
                        yield return "Debe seleccionar un rol para los supervisores.";
                    }

                    break;

                case nameof(IsFirst):
                    if (IsFirst && IsLast)
                    {
                        yield return "Area no puede ser primera y ultima a la vez >:(";
                    }

                    break;

                case nameof(IsLast):
                    if (IsFirst && IsLast)
                    {
                        yield return "Area no puede ser primera y ultima a la vez >:(";
                    }

                    break;
            }
        }
    }
}
