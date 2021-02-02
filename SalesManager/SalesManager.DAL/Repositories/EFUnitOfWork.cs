using SalesManager.DAL.EF;
using SalesManager.DAL.Entities;
using SalesManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private SalesDBContext _db;
        private SaleRepository _saleRepository;
        private ManagerRepository _managerRepository;
        private bool _disposed = false;

        public EFUnitOfWork(string connectionString)
        {
            _db = new SalesDBContext(connectionString);
        }

        public IRepository<Manager> Managers
        {
            get
            {
                if (_managerRepository == null)
                    _managerRepository = new ManagerRepository(_db);
                return _managerRepository;
            }
        }

        public IRepository<Sale> Sales
        {
            get
            {
                if (_saleRepository == null)
                    _saleRepository = new SaleRepository(_db);
                return _saleRepository;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _db.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void CreateDataBase()
        {
            try
            {
                _db.Database.CreateIfNotExists();
                Console.WriteLine("DataBase connected!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
