
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
        private const int PORT = 100;
        private readonly Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public bool ConnectedServer
        {
            get
            {
                return ClientSocket.Connected;
            }
        }
        public void ConnectToServer()
        {

            while (!ClientSocket.Connected)
            {
                try
                {
                    ClientSocket.Connect(IPAddress.Loopback, PORT);
                }
                catch (SocketException)
                {

                }
            }
        }


        public void SendMessage(string text)
        {
            if (ClientSocket.Connected)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(text);
                ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
        }

        public string ReceiveResponse()
        {
            try
            {
                if (ClientSocket.Connected)
                {
                    var buffer = new byte[2048];
                    int received = ClientSocket.Receive(buffer, SocketFlags.None);
                    if (received == 0)
                    {
                        return "";
                    }
                    var data = new byte[received];
                    Array.Copy(buffer, data, received);
                    string text = Encoding.ASCII.GetString(data);

                    return text;
                }
                else
                {
                    return "Bağlantı yok";
                }
            }
            catch (Exception ex)
            {

                return "Bağlantı yok";
            }

        }
    }
}
