// <copyright file="WorkOrderUnitWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkOrderUnitWrapper : ModelWrapper<WorkOrderUnit>
    {
        public WorkOrderUnitWrapper()
            : base(new WorkOrderUnit())
        {
        }

        public WorkOrderUnitWrapper(WorkOrderUnit model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public int WorkOrderId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public int WorkUnitId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public bool Finished
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public DateTime? FinishedDateTime
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            yield return null;

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
