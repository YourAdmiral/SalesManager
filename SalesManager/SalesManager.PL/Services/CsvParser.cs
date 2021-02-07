﻿using SalesManager.PL.Classes;
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
    public class CsvParser
    {
        private string _errorFilesPath;
        private Regex regex = new Regex(@"[a-я]*", RegexOptions.IgnoreCase);
        public ConcurrentDictionary<string, ManagerViewModel> ManagersViewModel { get; set; }

        public CsvParser(string errorFilesPath)
        {
            ManagersViewModel = new ConcurrentDictionary<string, ManagerViewModel>();
            _errorFilesPath = errorFilesPath;
        }

        public void Parse(string filePath)
        {
            string currentLine;
            bool check = false;
            ICollection<SaleViewModel> salesViewModel = new List<SaleViewModel>();
            StreamReader reader = new StreamReader(filePath);
            try
            {
                while ((currentLine = reader.ReadLine()) != null)
                {
                    salesViewModel.Add(GetSaleInfo(currentLine));
                }
                AddSaleInfo(filePath, salesViewModel);

            }
            catch (Exception e)
            {
                Console.WriteLine($"{Path.GetFileName(filePath)} cannot be read: {e.Message}");
                check = true;
            }
            finally
            {
                FinishParse(reader, check, filePath);
            }
        }

        public SaleViewModel GetSaleInfo(string currentLine)
        {
            string[] sailesInfo = currentLine.Split(';');
            return new SaleViewModel()
            {
                Date = DateTime.ParseExact(sailesInfo[0], "ddmmyyyy", null),
                Client = sailesInfo[1],
                Product = sailesInfo[2],
                Cost = Convert.ToInt32(sailesInfo[3])
            };
        }

        public void FinishParse(StreamReader reader, bool check, string filePath)
        {
            if (reader != null)
                reader.Close();

            if (check)
            {
                string path = Path.Combine(_errorFilesPath, Path.GetFileName(filePath));
                if (!File.Exists(path))
                    File.Move(filePath, path);
                else
                    File.Delete(filePath);
            }
        }

        public void AddSaleInfo(string filePath, ICollection<SaleViewModel> salesViewModel)
        {
            ManagerViewModel managerViewModel = new ManagerViewModel()
            {
                Name = regex.Match(Path.GetFileName(filePath)).Value,
                FilePath = filePath
            };
            managerViewModel.Sales = salesViewModel;
            ManagersViewModel.GetOrAdd(managerViewModel.Name, managerViewModel);
        }
    }
}
