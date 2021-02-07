using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SalesManager.PL.Managers
{
    public class FoldersManager
    {
        public string SourceFilesPath { get; private set; }
        public string ReadyFilesPath { get; private set; }
        public string ErrorFilesPath { get; private set; }

        public FoldersManager()
        {
            CreateFolders();
        }

        public void CreateFolders()
        {
            if (Directory.Exists(ConfigurationManager.AppSettings["SalesSourcePath"]))
            {
                SourceFilesPath = ConfigurationManager.AppSettings["SalesSourcePath"];
                ReadyFilesPath = ConfigurationManager.AppSettings["SalesReadyFilesPath"];
                ErrorFilesPath = ConfigurationManager.AppSettings["SalesErrorFilesPath"];
            }
            else
            {
                SourceFilesPath = CreateFolder(ConfigurationManager.AppSettings["SalesSourcePath"]);
                ReadyFilesPath = CreateFolder(ConfigurationManager.AppSettings["SalesReadyFilesPath"]);
                ErrorFilesPath = CreateFolder(ConfigurationManager.AppSettings["SalesErrorFilesPath"]);
            }
        }

        public string CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
