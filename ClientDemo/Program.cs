using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace ClientDemo
{

    class Program
    {
        static Client client;

        static void Main()
        {
            client = new Client();

            Console.Title = Process.GetCurrentProcess().ProcessName;
            client.ConnectToServer();
            if (client.ConnectedServer)
            {
                Console.Clear();
                Console.WriteLine("Connected");
                int sayac = 0;

                while (sayac < 4)
                {
                    if (client.ConnectedServer)
                    {
                        sayac++;

                        client.SendMessage("get time");
                        Console.WriteLine(client.ReceiveResponse());

                        Thread.Sleep(10);
                    }
                    else
                    {
                        Console.WriteLine("Server ile bağlantı kapalı");
                        break;
                    }

                }
            }
            else
            {
                Console.WriteLine("Server ile bağlantı kapalı");
            }

            Console.ReadLine();


        }
    }
}