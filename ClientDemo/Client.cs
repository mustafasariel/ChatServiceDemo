using ChatServiceCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientDemo
{
    public class Client
    {
        Socket socket;
        SocketError socketError;
        IPEndPoint iPEndPoint;

        byte[] buffer = new byte[1024];

        public Client(IPEndPoint iPEndPoint)
        {
            this.iPEndPoint = iPEndPoint;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            socket.BeginConnect(iPEndPoint, onBeginConnect, null); // !!! state parametresini kullanmak bir seçenek olabilir

        }

        private void onBeginConnect(IAsyncResult ar)
        {
            socket.EndConnect(ar);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onBeginReceive, null);

        }

        private void onBeginReceive(IAsyncResult ar)
        {
            int dataLenght = socket.EndReceive(ar, out socketError);

            if (dataLenght <= 0 || socketError != SocketError.Success)
            {
                Console.WriteLine("Bağlantı koptu");
                return;
            }
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onBeginReceive, null);
        }

        public void Send(ClientServerMessage message)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, message);
                IList<ArraySegment<byte>> data = new List<ArraySegment<byte>>();

                data.Add(new ArraySegment<byte>(ms.ToArray()));

                socket.BeginSend(data, SocketFlags.None, out socketError, sender, null);

                if (socketError != SocketError.Success)
                {
                    Console.WriteLine("Bağlantı koptu");
                }
            }
        }

        private void sender(IAsyncResult ar)
        {
            int dataLenght = socket.EndSend(ar, out socketError);
            if (dataLenght <= 0 || socketError != SocketError.Success)
            {
                Console.WriteLine("Bağlantı koptu");
                return;
            }
            else
            {
                Console.WriteLine($"dataLenght : {dataLenght}");
            }

        }
    }
}
