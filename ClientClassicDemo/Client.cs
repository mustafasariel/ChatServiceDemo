using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientClassicDemo
{
    public class Client
    {
        private const int PORT = 7200;
        public readonly Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public bool ConnectedServer
        {
            get
            {
                return ClientSocket.Connected;
            }
        }
        public void ConnectToServer()
        {

            //while (!ClientSocket.Connected)
            //{
            try
            {
                ClientSocket.Connect(IPAddress.Loopback, PORT);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            // }
        }

        /// <summary>
        /// string mesaj gönderen metot
        /// </summary>
        /// <param name="text"></param>
        public void SendMessage(string text)
        {
            if (ClientSocket.Connected)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(text);
                ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
        }
        /// <summary>
        /// serverın verdiği response alan metot
        /// </summary>
        /// <returns></returns>
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
