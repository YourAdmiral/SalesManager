using SalesManager.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Manager> Managers { get; }
        IRepository<Sale> Sales { get; }
        void Save();
        void CreateDatabase();
    }
}
