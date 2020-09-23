﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;

namespace SistemaMirno.UI.Data.Reports
{
    [Serializable]
    public class DeliveryOrderReport
    { 
        public string Date { get; set; }
        
        public string Responsible { get; set; }

        public bool IsNew { get; set; }

        public bool IsTransfer { get; set; }

        public string Branch { get; set; }

        public List<ClientReport> Clients { get; set; } = new List<ClientReport>();
    }
}
