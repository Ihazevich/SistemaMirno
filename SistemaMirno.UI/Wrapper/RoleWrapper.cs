﻿using System.Collections.Generic;
using System.IO;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class RoleWrapper : ModelWrapper<Role>
    {
        public RoleWrapper()
            : base(new Role())
        {
        }

        public RoleWrapper(Role model)
            : base(model)
        {
        }

        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the User name.
        /// </summary>
        public string Description
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the User password.
        /// </summary>
        public int BranchId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public bool HasAccessToSales
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool HasAccessToAccounting
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool HasAccessToHumanResources
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool HasAccessToProduction
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool HasAccessToLogistics
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool IsSystemAdmin
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public string ProceduresManualPdfFile
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool HasProceduresManual
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
                if (!value)
                {
                    ProceduresManualPdfFile = string.Empty;
                }
            }
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Description):
                    if (Description.Length < 4)
                    {
                        yield return "La descripcion es muy corto.";
                    }

                    break;

                case nameof(BranchId):
                    if (BranchId < 1)
                    {
                        yield return "Debe seleccionar una sucursal";
                    }

                    break;

                case nameof(HasProceduresManual):
                    if (HasProceduresManual && !File.Exists(ProceduresManualPdfFile))
                    {
                        yield return "Debe seleccionar un archivo valido";
                    }

                    break;

                case nameof(ProceduresManualPdfFile):
                    if (HasProceduresManual)
                    {
                        if (!File.Exists(ProceduresManualPdfFile))
                        {
                            yield return "El archivo no existe";
                        }
                        else if(!ProceduresManualPdfFile.Contains(".pdf"))
                        {
                            yield return "El manual debe ser un archivo pdf";
                        }
                        else
                        {
                            HasProceduresManual = true;
                        }
                    }
                    else if (!HasProceduresManual && ProceduresManualPdfFile.Length > 0)
                    {
                        yield return "Para seleccionar un archivo debe indicar que el rol posee un manual";
                    }
                    
                    break;
            }
        }
    }
}
