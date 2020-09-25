// <copyright file="ColorWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    public class ColorWrapper : ModelWrapper<Color>
    {
        public ColorWrapper()
            : base(new Color())
        {
        }

        public ColorWrapper(Color model)
            : base(model)
        {
        }

        public int Id => GetValue<int>();

        public string Name
        {
            get => GetValue<string>();
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
