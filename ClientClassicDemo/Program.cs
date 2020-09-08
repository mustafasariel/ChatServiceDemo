using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using System.Timers;

namespace ClientClassicDemo
{
    class Program
    {
        static Client client;
       static Timer timer;
        static void Main(string[] args)
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;

            client = new Client();
            client.ConnectToServer();


        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (client.ConnectedServer)
            {
                client.SendMessage($"Date:{DateTime.Now.ToString()}");

                Console.WriteLine(client.ReceiveResponse());
            }
        }
    }
}
