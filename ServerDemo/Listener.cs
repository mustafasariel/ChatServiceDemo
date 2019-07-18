using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using ChatServiceCore;

namespace ServerDemo
{
    public class Listener
    {

        Socket socket;
        int port;
        int maxConnectionCount;


        List<Client> ListClients = new List<Client>();


        public Listener(int port,int maxConnectionCount)
        {
            this.port = port;
            this.maxConnectionCount = maxConnectionCount;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, port);// any diyerek herhangi bir yerden demiş olduk.
            socket.Bind(iPEndPoint);
            socket.Listen(maxConnectionCount);// socketten gelen bağlantıları dinlemeye başlıyoruz.
            socket.BeginAccept(OnBeginAccept, socket);//asenkron olarak gelen bağlantıları alıyoruz.

        }

        private void OnBeginAccept(IAsyncResult ar)
        { 

            Client client = new Client(socket.EndAccept(ar));
            ListClients.Add(client);

            client.eventMessageReceived += new OnMessageReceived(MessageReceived);

            client.Start();
            socket.BeginAccept(OnBeginAccept, null);
        }

        private void MessageReceived(ClientServerMessage msg)
        {
            Console.WriteLine($"{msg.UserName} {msg.Message}");
        }
    }
}
