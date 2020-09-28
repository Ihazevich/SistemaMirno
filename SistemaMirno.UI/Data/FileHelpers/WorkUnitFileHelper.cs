// <copyright file="WorkUnitFileHelper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using FileHelpers;

namespace SistemaMirno.UI.Data.FileHelpers
{
    /// <summary>
    /// A class representing the <see cref="Model.WorkUnit"/> class mapping for use when reading from csv files.
    /// </summary>
    [IgnoreFirst]
    [DelimitedRecord("|")]
    public class WorkUnitFileHelper
    {
#pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// The name of the product.
        /// </summary>
        public string Product;

        /// <summary>
        /// The name of the material.
        /// </summary>
        public string Material;

        /// <summary>
        /// The name of the color.
        /// </summary>
        public string Color;

        /// <summary>
        /// The name of the current work area the work unit is at.
        /// </summary>
        public string CurrentArea;

        /// <summary>
        /// The name of the branch the work unit is at.
        /// </summary>
        public string Branch;

        /// <summary>
        /// A value indicating whether the work unit is custom or not.
        /// </summary>
        [FieldConverter(ConverterKind.Int32)]
        public int IsCustom;
#pragma warning restore SA1401 // Fields should be private
    }
}
