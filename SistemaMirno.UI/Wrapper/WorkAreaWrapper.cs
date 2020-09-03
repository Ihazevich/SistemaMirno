// <copyright file="WorkAreaWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using SistemaMirno.Model;

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

        public int Id => GetValue<int>();

        /// <summary>
        /// Gets or sets the User name.
        /// </summary>
        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        /// <summary>
        /// Gets or sets the User password.
        /// </summary>
        public string Position
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int BranchId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int ResponsibleRoleId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int SupervisorRoleId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public bool ReportsInProcess
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsLast
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsFirst
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsPassthrough
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public int? PassthroughWorkAreaId
        {
            get => GetValue<int?>();
            set => SetValue(value);
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

                case nameof(IsPassthrough):
                    if (IsPassthrough && PassthroughWorkAreaId < 1)
                    {
                        yield return "Debe elegir un area de destino";
                    }

                    break;

                case nameof(PassthroughWorkAreaId):
                    if (IsPassthrough && PassthroughWorkAreaId < 1)
                    {
                        IsPassthrough = true;
                        yield return "Debe elegir un area de destino";
                    }

                    break;
            }

            foreach (var error in base.ValidateProperty(propertyName))
            {
                if (error != null)
                {
                    yield return error;
                }
            }
        }
    }
}
