﻿// <copyright file="ColorWrapper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// A class representing the wrapper for the Color class.
    /// </summary>
    public class ColorWrapper : ModelWrapper<Color>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorWrapper"/> class.
        /// </summary>
        /// <param name="model">Instance of <see cref="Color"> to use as model.</param>
        public ColorWrapper(Color model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets the Color ID.
        /// </summary>
        public int Id { get { return GetValue<int>(); } }

        /// <summary>
        /// Gets or sets the Color name.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the collection of Work Units related to the color.
        /// </summary>
        public virtual Collection<WorkUnit> WorkUnits
        {
            get { return GetValue<Collection<WorkUnit>>(); }
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