using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.BLL.DTO
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ClientName { get; set; }
        public int Cost { get; set; }
        public DateTime Date { get; set; }
        public int ManagerId { get; set; }
    }
}
