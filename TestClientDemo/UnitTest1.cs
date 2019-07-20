using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestClientDemo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void receive_alerts_when_you_send_two_messages_in_one_second()
        {
            // test 1 saniye içinde iki defa mesaj gönderilirse uyarı verecek
            // serverın ayakta olması gerekiyor.
            //Arrange
            string beklenen = "uyari";
            string sonuc = "";

            ClientDemo.Client client = new ClientDemo.Client();

            //Act
            client.ConnectToServer();
            client.SendMessage("saatkac");
            sonuc = client.ReceiveResponse();

            Thread.Sleep(10);// sadece 10 milisaniye bekledikten sonra ikinci komutu gönderiyorum.
            client.SendMessage("saatkac");

            sonuc = client.ReceiveResponse();


            //Assert
            Assert.AreEqual(beklenen, sonuc);

         
        }

        [TestMethod]
        public void When_you_send_two_messages_in_one_second_connection_breaks_again_after_receiving_warning()
        {
            // uyarı almasına rağmen tekrar 1 sn içinde iki mesaj gönderirse disconnect olacak.
            // // serverın ayakta olması gerekiyor.

            try
            {
                //Arrange

                ClientDemo.Client client = new ClientDemo.Client();

                //Act

                client.ConnectToServer();

                for (int i = 0; i < 10; i++)
                {
                    if (client.ClientSocket.Connected)
                    {
                        client.SendMessage("saatkac");
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {
                //Assert
                Assert.IsTrue(ex is System.Net.Sockets.SocketException);
            }
        }
    }
}
