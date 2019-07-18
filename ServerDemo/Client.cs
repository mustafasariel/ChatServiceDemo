using ChatServiceCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ServerDemo
{
    public delegate void OnMessageReceived(ClientServerMessage msg);

    public class Client
    {
        SocketError socketError;
        byte[] buffer = new byte[1024];

        public OnMessageReceived eventMessageReceived;
        Socket socket;

        public Client(Socket socket)
        {
            this.socket = socket;
        }

        DateTime LastMessageDate { get; set; }

        public void Start()
        {
            // data dinlemeye başlıyoruz.
            this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnBeginReceiveCallback, null);
        }

        private void OnBeginReceiveCallback(IAsyncResult ar)
        {
            int datalength = socket.EndReceive(ar, out socketError);

            byte[] byteArray = new byte[datalength];

            Array.Copy(buffer, 0, byteArray, 0, byteArray.Length);

            ParseData(byteArray);

            Start();
        }

        private void ParseData(byte[] byteArray)
        {
            try
            {
                if (eventMessageReceived != null)
                {
                    using (var ms = new MemoryStream(byteArray))
                    {
                        ClientServerMessage msg = new BinaryFormatter().Deserialize(ms) as ClientServerMessage;

                        eventMessageReceived(msg);
                    }
                }
            }
            
            catch (Exception ex)
            {

                Console.WriteLine( ex.ToString());
            }
            
        }
    }
}
