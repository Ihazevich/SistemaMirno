// <copyright file="ProductFileHelper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using FileHelpers;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.FileHelpers
{
    /// <summary>
    /// A class representing the <see cref="Product"/> class mapping for use when reading from csv files.
    /// </summary>
    [IgnoreFirst]
    [DelimitedRecord("|")]
    public class ProductFileHelper
    {
        public string Code;

        public string Name;

        public string Category;

        [FieldConverter(ConverterKind.Int64)]
        public long ProductionValue;

        [FieldConverter(ConverterKind.Int64)]
        public long WholesalerPrice;

        [FieldConverter(ConverterKind.Int64)]
        public long RetailPrice;
    }
}