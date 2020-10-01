// <copyright file="Role.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMirno.Model
{
    /// <summary>
    /// Represents a company role.
    /// </summary>
    public class Role : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Descripción requerida.")]
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the id of the related <see cref="Model.Branch"/> entity.
        /// </summary>
        [Required]
        public int BranchId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Branch"/> entity.
        /// </summary>
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role has access to the sales module.
        /// </summary>
        [Required]
        public bool HasAccessToSales { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role has access to the production module.
        /// </summary>
        [Required]
        public bool HasAccessToProduction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role has access to the human resources module.
        /// </summary>
        [Required]
        public bool HasAccessToHumanResources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role has access to the accounting module.
        /// </summary>
        [Required]
        public bool HasAccessToAccounting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role has access to the logistics module.
        /// </summary>
        [Required]
        public bool HasAccessToLogistics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is a system administrator.
        /// </summary>
        [Required]
        public bool IsSystemAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is from the human resources department.
        /// </summary>
        public bool IsFromHumanResourcesDepartment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is from the production department.
        /// </summary>
        public bool IsFromProductionDepartment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is from the sales department.
        /// </summary>
        public bool IsFromSalesDepartment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is from the logistics department.
        /// </summary>
        public bool IsFromLogisticsDepartment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is from the information techonology department.
        /// </summary>
        public bool IsFromInformationTechnologyDepartment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is from the accounting department.
        /// </summary>
        public bool IsFromAccountingDepartment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role is from the management department.
        /// </summary>
        public bool IsFromManagementDepartment { get; set; }

        /// <summary>
        /// Gets or sets the location of the procedures manual file.
        /// </summary>
        public string ProceduresManualPdfFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role has a procedures manual.
        /// </summary>
        [Required]
        public bool HasProceduresManual { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entities.
        /// </summary>
        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.WorkArea"/> entities
        /// which have this role as the responsible role.
        /// </summary>
        [ForeignKey(nameof(WorkArea.ResponsibleRoleId))]
        public virtual ICollection<WorkArea> ResponsibleOfWorkAreas { get; set; } = new HashSet<WorkArea>();

        /// <summary>
        /// Gets or sets the navigation property to the related <see cref="Model.Employee"/> entities
        /// which have this role as the supervisor role.
        /// </summary>
        [ForeignKey(nameof(WorkArea.SupervisorRoleId))]
        public virtual ICollection<WorkArea> SupervisorOfWorkAreas { get; set; } = new HashSet<WorkArea>();
    }
}
