﻿using SalesManager.DAL.EF;
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
    class ManagerRepository : IRepository<Manager>
    {
        private SalesDBContext _db;

        public ManagerRepository(SalesDBContext dbContext)
        {
            _db = dbContext;
        }

        public void Create(Manager item)
        {
            _db.Managers.Add(item);
        }

        public void Update(Manager item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Manager manager = _db.Managers.Find(id);
            if (manager != null)
                _db.Managers.Remove(manager);
        }

        public Manager Get(int id)
        {
            return _db.Managers.Find(id);
        }

        public IEnumerable<Manager> GetAll()
        {
            return _db.Managers;
        }

        public IEnumerable<Manager> Find(Func<Manager, bool> predicate)
        {
            return _db.Managers.Where(predicate).ToList();
        }
    }
}
