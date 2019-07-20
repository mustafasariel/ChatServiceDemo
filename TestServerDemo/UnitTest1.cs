using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestServerDemo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            ServerDemo.Listener listener = new ServerDemo.Listener();
            listener.Start();
            ClientDemo.Client client1 = new ClientDemo.Client();
            ClientDemo.Client client2 = new ClientDemo.Client();
            ClientDemo.Client client3 = new ClientDemo.Client();

            client1.ConnectToServer();
            client2.ConnectToServer();
            client3.ConnectToServer();
            client1.SendMessage("saatkac");
            client2.SendMessage("saatkac");
            client3.SendMessage("saatkac");

            Assert.AreEqual(3, listener.);
        }
    }
}
