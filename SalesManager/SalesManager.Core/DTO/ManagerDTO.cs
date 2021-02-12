using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.Core.DTO
{
    public class ManagerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SaleDTO> Sales { get; set; }
    }
}
