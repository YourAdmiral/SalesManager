﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.PL.Classes
{
    public class SaleViewModel
    {
        public string Product { get; set; }
        public string Client { get; set; }
        public int Cost { get; set; }
        public DateTime Date { get; set; }
    }
}
