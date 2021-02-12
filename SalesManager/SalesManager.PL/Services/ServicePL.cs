using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using AutoMapper;
using Ninject;
using SalesManager.BLL.Interfaces;
using SalesManager.PL.Managers;
using SalesManager.PL.Models;
using SalesManager.Core.DTO;

namespace SalesManager.PL.Services
{
    public class ServicePL
    {
        private string _connectionString;
        private IServiceBLL _serviceBLL;
        private FilesManager _filesManager;
        private StandardKernel _kernel;

        public event Action TimerStart;
        public event Action TimerStop;

        public ServicePL()
        {
            _filesManager = new FilesManager(this);

            _connectionString = ConfigurationManager.ConnectionStrings["SalesDbConnectionString"].ConnectionString;
            _kernel = new StandardKernel(new ServiceModulePL(_connectionString));
            _serviceBLL = _kernel.Get<IServiceBLL>();

            TimerStart += _filesManager.StartTimer;
            TimerStop += _filesManager.StopTimer;
        }

        public void StartClient()
        {
            _serviceBLL.CheckDatabase();
            TimerStart?.Invoke();
        }

        public void StopClient()
        {
            TimerStop?.Invoke();
        }

        public void UnsubscribeFromEvents()
        {
            TimerStart -= _filesManager.StartTimer;
            TimerStop -= _filesManager.StopTimer;
        }

        public void HandleViewModel(ManagerViewModel manager)
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<ManagerViewModel, ManagerDTO>(); cfg.CreateMap<SaleViewModel, SaleDTO>(); });
            ManagerDTO managerDTO = Mapper.Map<ManagerViewModel, ManagerDTO>(manager);
            _serviceBLL.HandleData(managerDTO);
        }
    }
}
