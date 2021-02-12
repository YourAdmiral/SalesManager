using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.PL.Models
{
    public class SaleViewModel
    {
        public string ProductName { get; set; }
        public string ClientName { get; set; }
        public int Cost { get; set; }
        public DateTime Date { get; set; }
    }
}
