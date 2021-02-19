﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesManager.PL.Services;

namespace SalesManager
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var service = new Scanner();
                service.StartClient();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
