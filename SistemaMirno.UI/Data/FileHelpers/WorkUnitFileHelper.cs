using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
