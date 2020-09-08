using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerDemo2
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Listener
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly Dictionary<string,Socket> clientSockets = new Dictionary<string, Socket>();
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 7200;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        public int ClientCount
        {
            get { return clientSockets.Count; }
        }
        /// <summary>
        /// clientların bağlantı taleplerini asenkron olarak dinliyen metot
        /// </summary>
        public void Start()
        {
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult asyncResult)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(asyncResult);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            clientSockets.Add("", socket);
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            serverSocket.BeginAccept(AcceptCallback, null);
        }
        /// <summary>
        /// iş kuralını gerçekleyen metot.
        /// Kural 1 saniye içinde iki mesaj gönderene uyarı verilecek,
        /// uyarı sonrası tekrar aynı kuralı ihlal edenin bağlantısı kesilecek.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        SocketState BussinesRule(Socket current)
        {
            var cState = clientSockets[current];

            if (cState.ListMessageDate.Count == 0)
            {
                cState.AddMessageDate(DateTime.Now);// ilk mesajın zamanını ekledik.
                cState.RuleException = false;
            }
            else
            {

                TimeSpan timeSpan = DateTime.Now - cState.ListMessageDate[cState.ListMessageDate.Count - 1];
                if (timeSpan.TotalMilliseconds < 1000 && cState.RuleException == false)
                {
                    cState.RuleException = true;
                    return SocketState.warning;

                }
                else if (timeSpan.TotalMilliseconds < 1000 && cState.RuleException == true)
                {
                    cState.RuleException = true;
                    return SocketState.Disconnect;
                }
            }
            return SocketState.None;


        }
        /// <summary>
        /// client tarafından gelen mesajı alır ve geriye mesaj veya uyarı döndürür.
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                Socket current = (Socket)asyncResult.AsyncState;

                var rule = BussinesRule(current);

                if (rule == SocketState.warning)
                {
                    byte[] data = Encoding.ASCII.GetBytes("uyari");
                    current.Send(data);
                }
                else if (rule == SocketState.Disconnect)
                {
                    //byte[] data = Encoding.ASCII.GetBytes("disconnect");
                    //current.Send(data);

                    current.Shutdown(SocketShutdown.Both);
                    current.Close();
                    clientSockets.Remove(current);
                    return;
                }

                int received;

                try
                {
                    received = current.EndReceive(asyncResult);
                }
                catch (SocketException)
                {
                    current.Close();
                    clientSockets.Remove(current);
                    return;
                }

                byte[] recBuf = new byte[received];
                Array.Copy(buffer, recBuf, received);
                string text = Encoding.ASCII.GetString(recBuf);

                if (text.ToLower() == "saatkac")
                {
                    byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                    current.Send(data);
                }
                else
                {
                    byte[] data = Encoding.ASCII.GetBytes("hatali komut");
                    current.Send(data);
                }

                current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
            }
            catch (Exception)
            {
            }
        }
    }
}
