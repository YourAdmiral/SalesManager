using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesManager.PL.Services;
using System.Threading;
using SalesManager.PL.Models;

namespace SalesManager.PL.Managers
{
    public class FilesManager
    {
        private ICollection<string> _filesPaths;
        private ICollection<ManagerViewModel> _managersFiles;

        private CsvReader _csvReader;
        private ServicePL _servicePL;
        private FoldersManager _foldersManager;

        private Timer _timer;
        private TimerCallback _callBack;

        public FilesManager(ServicePL servicePL)
        {
            _foldersManager = new FoldersManager();
            _csvReader = new CsvReader(_foldersManager.ErrorFilesPath);
            _servicePL = servicePL;
        }

        public void CheckSourceFolder(object salesSourcePath)
        {
            _filesPaths = Directory.GetFiles((string)salesSourcePath, "*.csv").ToList();
            if (_filesPaths.Count != 0)
            {
                _timer.Change(Timeout.Infinite, 0);

                Parallel.ForEach(_filesPaths, _csvReader.Read);
                _managersFiles = _csvReader.ManagersViewModels.Values;
                Parallel.ForEach(_managersFiles, HandleFiles);

                _filesPaths.Clear();
                _csvReader.ManagersViewModels.Clear();
                _timer.Change(0, 1000);
            }
        }

        private void HandleFiles(ManagerViewModel manager)
        {
            _servicePL.HandleViewModel(manager);

            string targetPath = Path.Combine(_foldersManager.ReadyFilesPath, Path.GetFileName(manager.FilePath));
            if (!File.Exists(targetPath))
            {
                File.Move(manager.FilePath, targetPath);
            }
            else
            {
                File.Delete(manager.FilePath);
            }
        }

        public void StartTimer()
        {
            _callBack = new TimerCallback(CheckSourceFolder);
            _timer = new Timer(_callBack, _foldersManager.SourceFilesPath, 0, 1000);
            Console.WriteLine("Files tracking started!\n");
        }

        public void StopTimer()
        {
            _timer.Dispose();
            Console.WriteLine("Files tracking stopped!\n");
        }
    }
}
