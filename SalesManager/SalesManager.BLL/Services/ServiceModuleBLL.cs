using Ninject.Modules;
using SalesManager.DAL.Interfaces;
using SalesManager.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.BLL.Services
{
    public class ServiceModuleBLL : NinjectModule
    {
        private string _connectionString;

        public ServiceModuleBLL(string connection)
        {
            _connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument("connectionString", _connectionString);
        }
    }
}
