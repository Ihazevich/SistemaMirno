﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.Model
{
    public class Payment : ModelBase
    {
        public DateTime Date { get; set; }
        public int Ammount { get; set; }
        public int PaymentTypeId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public int SaleId { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
