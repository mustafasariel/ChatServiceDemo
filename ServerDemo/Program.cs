﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Listener listener = new Listener();
            Console.Title = "Server";
            listener.Start();
          


            Console.WriteLine($"Bağlı olan Clinet sayısı {listener.ClientCount}");

            Console.ReadLine();

        }
    }
}
