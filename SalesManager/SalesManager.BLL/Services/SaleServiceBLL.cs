using AutoMapper;
using Ninject;
using SalesManager.BLL.DTO;
using SalesManager.BLL.Interfaces;
using SalesManager.DAL.Entities;
using SalesManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManager.BLL.Services
{
    public class SaleServiceBLL : IServiceBLL
    {
        private StandardKernel _kernel;
        private IUnitOfWork _db;

        public SaleServiceBLL(string connectionString)
        {
            _kernel = new StandardKernel(new ServiceModuleBLL(connectionString));
            _db = _kernel.Get<IUnitOfWork>();
        }

        public void CheckDatabase()
        {
            _db.CreateDatabase();
            _db.Dispose();
        }

        public void HandleData(ManagerDTO managerDTO)
        {
            IUnitOfWork db = _kernel.Get<IUnitOfWork>();
            int? managerId = db.Managers.GetId(x => x.Name == managerDTO.Name);
            if (managerId == null)
                AddManager(managerDTO, db);
            else
                AddSale(managerDTO, (int)managerId, db);
            db.Dispose();
        }

        private void AddSale(ManagerDTO managerDTO, int managerId, IUnitOfWork db)
        {
            if (managerDTO.Sales.Count != 0)
            {
                Mapper.Initialize(cfg => { cfg.CreateMap<SaleDTO, Sale>().ForMember(dest => dest.Manager, option => option.Ignore()); } );
                ICollection<Sale> sales = Mapper.Map<ICollection<SaleDTO>, ICollection<Sale>>(managerDTO.Sales);
                foreach (Sale sale in sales)
                {
                    sale.ManagerId = (int)managerId;
                    db.Sales.Create(sale);
                }
                db.Save();
            }
        }

        private void AddManager(ManagerDTO managerDTO, IUnitOfWork db)
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<SaleDTO, Sale>().ForMember(dest => dest.Manager, option => option.Ignore()); cfg.CreateMap<ManagerDTO, Manager>(); } );
            Mapper.AssertConfigurationIsValid();
            Manager manager = Mapper.Map<ManagerDTO, Manager>(managerDTO);
            db.Managers.Create(manager);
            db.Save();
        }
    }
}
