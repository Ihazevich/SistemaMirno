// <copyright file="ProductFileHelper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using FileHelpers;

namespace SistemaMirno.UI.Data.FileHelpers
{
    /// <summary>
    /// A class representing the <see cref="Model.Product"/> class mapping for use when reading from csv files.
    /// </summary>
    [IgnoreFirst]
    [DelimitedRecord("|")]
    public class ProductFileHelper
    {
#pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// The code of the product.
        /// </summary>
        public string Code;

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string Name;

        /// <summary>
        /// The category of the product.
        /// </summary>
        public string Category;

        /// <summary>
        /// The production value of the product.
        /// </summary>
        [FieldConverter(ConverterKind.Int64)]
        public long ProductionValue;

        /// <summary>
        /// The wholesaler price of the product.
        /// </summary>
        [FieldConverter(ConverterKind.Int64)]
        public long WholesalerPrice;

        /// <summary>
        /// The retail price of the product.
        /// </summary>
        [FieldConverter(ConverterKind.Int64)]
        public long RetailPrice;
#pragma warning restore SA1401 // Fields should be private
    }
}