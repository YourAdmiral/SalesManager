using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesManager.DAL.EF;
using SalesManager.DAL.Interfaces;
using SalesManager.DAL.Repositories;

namespace SalesManager
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnitOfWork unitOfWork = new EFUnitOfWork("SalesDBConnectionString");
            unitOfWork.CreateDatabase();
            Console.ReadKey();
        }
    }
}
