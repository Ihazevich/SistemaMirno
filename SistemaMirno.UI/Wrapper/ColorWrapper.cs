﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public int Id
        {
            get { return GetValue<int>(); }
        }

        public string Name
        {
            get { return GetValue<string>(); }
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
