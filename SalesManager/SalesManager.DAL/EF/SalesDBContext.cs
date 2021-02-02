using SalesManager.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.DAL.EF
{
    public class SalesDBContext : DbContext
    {
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Manager> Managers { get; set; }

        public SalesDBContext() : base("SalesDBConnectionString")
        {

        }
    }
}
