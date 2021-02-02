using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesManager.DAL.EF;

namespace SalesManager
{
    class Program
    {
        static void Main(string[] args)
        {
            using (DbContext context = new SalesDBContext())
            {
                context.Database.CreateIfNotExists();
            }
        }
    }
}
