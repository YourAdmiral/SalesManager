using SalesManager.PL.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SalesManager.ServiceClient
{
    partial class Service1 : ServiceBase
    {
        private ServicePL _services;

        public Service1()
        {
            InitializeComponent();
            CanStop = true;
        }

        protected override void OnStart(string[] args)
        {
            _services = new ServicePL();
            Thread servicePLThread = new Thread(new ThreadStart(_services.StartClient));
            servicePLThread.Start();
        }

        protected override void OnStop()
        {
            _services.StopClient();
            Thread.Sleep(1000);
        }
    }
}
