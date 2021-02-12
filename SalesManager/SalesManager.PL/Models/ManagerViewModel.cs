using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.PL.Models
{
    public class ManagerViewModel
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public virtual ICollection<SaleViewModel> Sales { get; set; }
    }
}
