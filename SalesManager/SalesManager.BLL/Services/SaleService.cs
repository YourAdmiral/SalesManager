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
    public class SaleService : IService
    {
        private StandardKernel _kernel;
        private IUnitOfWork _db;

        public SaleService(string connection)
        {
            _kernel = new StandardKernel(new ServiceModule(connection));
            _db = _kernel.Get<IUnitOfWork>();
        }

        public void CheckDatabase()
        {
            _db.CreateDatabase();
            _db.Dispose();
        }

        public void HandleManagerInfo(ManagerDTO managerDTO)
        {
            IUnitOfWork db = _kernel.Get<IUnitOfWork>();
            int? managerId = db.Managers.GetId(x => x.Name == managerDTO.Name);
            if (managerId != null)
                AddSalesInfo(managerDTO, (int)managerId, db);
            else
                AddNewManager(managerDTO, db);
            db.Dispose();
        }

        private void AddSalesInfo(ManagerDTO managerDTO, int managerId, IUnitOfWork db)
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

        private void AddNewManager(ManagerDTO managerDTO, IUnitOfWork db)
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<SaleDTO, Sale>().ForMember(dest => dest.Manager, option => option.Ignore()); cfg.CreateMap<ManagerDTO, Manager>(); } );
            Mapper.AssertConfigurationIsValid();
            Manager manager = Mapper.Map<ManagerDTO, Manager>(managerDTO);
            db.Managers.Create(manager);
            db.Save();
        }
    }
}
