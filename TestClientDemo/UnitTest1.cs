using System;
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
        }

        [TestMethod]
        public void When_you_send_two_messages_in_one_second_connection_breaks_again_after_receiving_warning()
        {
            // uyarı almasına rağmen tekrar 1 sn içinde iki mesaj gönderirse disconnect olacak.
        }
    }
}
