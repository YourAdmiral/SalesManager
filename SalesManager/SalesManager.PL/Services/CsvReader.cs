using SalesManager.PL.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalesManager.PL.Services
{
    public class CsvReader
    {
        private string _errorFilesPath;
        private Regex regex = new Regex(@"[a-я]*", RegexOptions.IgnoreCase);
        public ConcurrentDictionary<string, ManagerViewModel> ManagersViewModels { get; set; }

        public CsvReader(string errorFilesPath)
        {
            _errorFilesPath = errorFilesPath;
            ManagersViewModels = new ConcurrentDictionary<string, ManagerViewModel>();
        }

        public void Parse(string filePath)
        {
            string currentLine;
            bool check = false;
            ICollection<SaleViewModel> salesViewModel = new List<SaleViewModel>();
            StreamReader reader = new StreamReader(filePath);
            try
            {
                Console.WriteLine($"Added file: {Path.GetFileName(filePath)}");
                while ((currentLine = reader.ReadLine()) != null)
                {
                    salesViewModel.Add(GetSaleInfo(currentLine));
                }
                AddSalesInfo(filePath, salesViewModel);
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{Path.GetFileName(filePath)} cannot be read: {e.Message}");
                check = true;
            }
            finally
            {
                FinishRead(reader, check, filePath);
            }
        }

        public SaleViewModel GetSaleInfo(string currentLine)
        {
            string[] sailesInfo = currentLine.Split(';');
            Console.WriteLine(sailesInfo[0] + "; " + sailesInfo[1] + "; " + sailesInfo[2] + "; " + sailesInfo[3]);
            return new SaleViewModel()
            {
                Date = DateTime.ParseExact(sailesInfo[0].Substring(1), "dd.MM.yyyy", null),
                ClientName = sailesInfo[1],
                ProductName = sailesInfo[2],
                Cost = Convert.ToInt32(sailesInfo[3].Remove(sailesInfo[3].Length-1))
            };
        }

        public void FinishRead(StreamReader reader, bool check, string filePath)
        {
            if (reader != null)
            {
                reader.Close();
            }

            if (check)
            {
                string path = Path.Combine(_errorFilesPath, Path.GetFileName(filePath));
                if (!File.Exists(path))
                {
                    File.Move(filePath, path);
                }
                else
                {
                    File.Delete(filePath);
                }
            }
        }

        public void AddSalesInfo(string filePath, ICollection<SaleViewModel> salesViewModel)
        {
            ManagerViewModel managerViewModel = new ManagerViewModel()
            {
                Name = regex.Match(Path.GetFileName(filePath)).Value,
                FilePath = filePath
            };
            managerViewModel.Sales = salesViewModel;
            ManagersViewModels.GetOrAdd(managerViewModel.Name, managerViewModel);
        }
    }
}
