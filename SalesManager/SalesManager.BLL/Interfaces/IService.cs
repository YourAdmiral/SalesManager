using SalesManager.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.BLL.Interfaces
{
    public interface IService
    {
        void CheckDatabase();
        void HandleManagerInfo(ManagerDTO managerDTO);
    }
}
