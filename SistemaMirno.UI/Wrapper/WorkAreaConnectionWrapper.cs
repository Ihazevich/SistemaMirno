// <copyright file="WorkAreaConnectionWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class WorkAreaConnectionWrapper : ModelWrapper<WorkAreaConnection>
    {
        public WorkAreaConnectionWrapper()
            : base(new WorkAreaConnection())
        {
        }

        public WorkAreaConnectionWrapper(WorkAreaConnection model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public int OriginWorkAreaId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int DestinationWorkAreaId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }
    }
}
