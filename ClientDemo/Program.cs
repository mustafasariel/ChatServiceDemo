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


        static void test1()
        {
            try
            {


                client = new Client();

                Console.Title = Process.GetCurrentProcess().ProcessName;
                client.ConnectToServer();
                if (client.ConnectedServer)
                {
                    Console.Clear();
                    Console.WriteLine("Bağlantı kuruldu");
                    int sayac = 0;

                    while (sayac < 2)
                    {
                        Console.WriteLine($"Bağlantı :{client.ClientSocket.Connected}");
                        if (client.ClientSocket.Connected)
                        {
                            sayac++;

                            client.SendMessage("saatkac");
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
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }
        static void Main()
        {

            //test1();
            client = new Client();
            client.ConnectToServer();
            client.SendMessage("saatkac");
            Console.WriteLine(client.ReceiveResponse());

            Console.ReadLine();
            // test2();
        }

        private static void test2()
        {
            Exception beklenen = new System.Net.Sockets.SocketException();
            Exception sonuc = null;
            try
            {


                ClientDemo.Client client = new ClientDemo.Client();


                client.ConnectToServer();

                for (int i = 0; i < 10; i++)
                {

                    client.SendMessage("saatkac");
                    Thread.Sleep(10);

                }
            }
            catch (Exception ex)
            {
                sonuc = ex;
            }

            if (sonuc.GetType().Name == beklenen.GetType().Name)
            {
                Console.WriteLine("beklenen hata oluştu");
            }
            Console.ReadLine();
        }
    }
}