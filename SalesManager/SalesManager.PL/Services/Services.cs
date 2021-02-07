using SalesManager.PL.Classes;
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
using SalesManager.BLL.DTO;
using SalesManager.PL.Managers;

namespace SalesManager.PL.Services
{
    public class Services
    {
        private string _connectionString;
        private ICollection<string> _filesPaths;
        private ICollection<ManagerViewModel> _managersFiles;
        private IServiceBLL _serviceBLL;

        private FoldersManager _foldersManager;
        private CsvParser _csvParser;
        private StandardKernel _kernel;

        private Timer _timer;
        private TimerCallback _callBack;

        public event Action TimerStart;
        public event Action TimerStop;

        public Services()
        {
            _foldersManager = new FoldersManager();

            _connectionString = ConfigurationManager.ConnectionStrings["SalesDbConnectionString"].ConnectionString;
            _csvParser = new CsvParser(_foldersManager.ErrorFilesPath);
            _kernel = new StandardKernel(new ServiceModulePL(_connectionString));
            _serviceBLL = _kernel.Get<IServiceBLL>();

            TimerStart += StartTimer;
            TimerStop += StopTimer;
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

        private void StartTimer()
        {
            _callBack = new TimerCallback(CheckSourceFolder);
            _timer = new Timer(_callBack, _foldersManager.SourceFilesPath, 0, 1000);
            Console.WriteLine("Console client started!\n");
        }

        private void StopTimer()
        {
            _timer.Dispose();
            Console.WriteLine("Console client stopped!\n");
        }

        private void CheckSourceFolder(object salesSourcePath)
        {
            _filesPaths = Directory.GetFiles((string)salesSourcePath, "*.csv").ToList();
            if (_filesPaths.Count != 0)
            {
                _timer.Change(Timeout.Infinite, 0);

                Parallel.ForEach(_filesPaths, _csvParser.Parse);
                _managersFiles = _csvParser.ManagersViewModel.Values;
                Parallel.ForEach(_managersFiles, HandleFiles);

                _filesPaths.Clear();
                _csvParser.ManagersViewModel.Clear();
                _timer.Change(0, 1000);
            }
        }

        private void HandleFiles(ManagerViewModel manager)
        {
            InputIntoDb(manager);
            string targetPath = Path.Combine(_foldersManager.ReadyFilesPath, Path.GetFileName(manager.FilePath));

            if (!File.Exists(targetPath))
                File.Move(manager.FilePath, targetPath);
            else
                File.Delete(manager.FilePath);
        }

        private void InputIntoDb(ManagerViewModel manager)
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<ManagerViewModel, ManagerDTO>(); cfg.CreateMap<SaleViewModel, SaleDTO>(); });
            ManagerDTO managerDTO = Mapper.Map<ManagerViewModel, ManagerDTO>(manager);
            _serviceBLL.HandleManagerInfo(managerDTO);
        }
    }
}
