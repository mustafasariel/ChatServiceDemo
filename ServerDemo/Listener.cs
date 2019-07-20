using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ServerDemo
{

    public class Listener
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly Dictionary<Socket, SocketStateObj> clientSockets = new Dictionary<Socket, SocketStateObj>();
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 100;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        public void Start()
        {
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            clientSockets.Add(socket, new SocketStateObj());
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            serverSocket.BeginAccept(AcceptCallback, null);
        }

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
        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                Socket current = (Socket)AR.AsyncState;

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
                    received = current.EndReceive(AR);
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
                Console.WriteLine("Received Text: " + text);

                if (text.ToLower() == "get time")
                {
                    byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                    current.Send(data);
                }
                else
                {
                    byte[] data = Encoding.ASCII.GetBytes("Invalid request");
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
