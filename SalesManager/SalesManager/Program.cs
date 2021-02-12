using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesManager.PL.Services;

namespace SalesManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new ServicePL();
            service.StartClient();
            Console.ReadKey();
        }
    }
}
