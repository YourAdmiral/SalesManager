using SalesManager.DAL.EF;
using SalesManager.DAL.Entities;
using SalesManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.DAL.Repositories
{
    public class SaleRepository : IRepository<Sale>
    {
        private SalesDBContext _db;

        public SaleRepository(SalesDBContext db)
        {
            _db = db;
        }

        public void Create(Sale item)
        {
            _db.Sales.Add(item);
        }

        public void Update(Sale item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Sale sale = _db.Sales.Find(id);
            if (sale != null)
                _db.Sales.Remove(sale);
        }

        public Sale Get(int id)
        {
            return _db.Sales.Find(id);
        }

        public IEnumerable<Sale> GetAll()
        {
            return _db.Sales.Include(o => o.Manager);
        }

        public IEnumerable<Sale> Find(Func<Sale, bool> predicate)
        {
            return _db.Sales.Include(o => o.Manager).Where(predicate).ToList();
        }

        public int? GetId(Func<Sale, bool> predicate)
        {
            Sale sale = _db.Sales.FirstOrDefault(predicate);
            if (sale != null)
                return sale.Id;
            return null;
        }
    }
}
