using Ninject.Modules;
using SalesManager.BLL.Interfaces;
using SalesManager.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.PL.Services
{
    public class ServiceModulePL : NinjectModule
    {
        private string _connectionString;

        public ServiceModulePL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<IServiceBLL>().To<SaleServiceBLL>().WithConstructorArgument("connectionString", _connectionString);
        }
    }
}
