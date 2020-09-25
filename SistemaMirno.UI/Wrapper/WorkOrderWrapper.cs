// <copyright file="WorkOrderWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkOrderWrapper : ModelWrapper<WorkOrder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderWrapper"/> class.
        /// </summary>
        public WorkOrderWrapper()
            : base(new WorkOrder())
        {
        }

        public WorkOrderWrapper(WorkOrder model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public DateTime CreationDateTime
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime? FinishedDateTime
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public int OriginWorkAreaId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int DestinationWorkAreaId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int ResponsibleEmployeeId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int SupervisorEmployeeId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string Observations
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(ResponsibleEmployeeId):
                    if (ResponsibleEmployeeId < 1)
                    {
                        yield return "Debe seleccionar un responsable.";
                    }

                    break;

                case nameof(SupervisorEmployeeId):
                    if (SupervisorEmployeeId < 1)
                    {
                        yield return "Debe seleccionar un supervisor.";
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
