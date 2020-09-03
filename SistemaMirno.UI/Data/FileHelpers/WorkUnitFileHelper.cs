// <copyright file="WorkUnitFileHelper.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using FileHelpers;

namespace SistemaMirno.UI.Data.FileHelpers
{
    [IgnoreFirst]
    [DelimitedRecord("|")]
    public class WorkUnitFileHelper
    {
        public string Product;

        public string Material;

        public string Color;

        public string CurrentArea;

        public string Branch;

        [FieldConverter(ConverterKind.Int32)]
        public int IsCustom;
    }
}
