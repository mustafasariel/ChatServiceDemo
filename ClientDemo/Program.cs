using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ChatServiceCore;

namespace ClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            Client client = new Client(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));

            client.Start();

            ClientServerMessage message = new ClientServerMessage();

            Console.WriteLine("Usernmame:");
            message.UserName = Console.ReadLine();
            Console.WriteLine("Çıkmak için -1 yazınız");


            while (true)
            {
                Console.WriteLine("Mesajınız");
                string mesaj = Console.ReadLine();
                if (mesaj=="-1")
                {
                    Console.WriteLine("Çıkış yaptınız...");
                    break;
                }
                else
                {
                    message.Message = mesaj;
                    client.Send(message);
                }
                
            }
            Console.ReadLine();
        }
    }
}
