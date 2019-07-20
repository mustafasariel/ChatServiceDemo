using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerDemo
{
    public enum SocketState
    {
        None,
        warning,
        Disconnect
    }
    public class SocketStateObj
    {

        public List<DateTime> ListMessageDate { get; }
        public SocketStateObj()
        {
            ListMessageDate = new List<DateTime>();
        }
        public void AddMessageDate(DateTime dateTime)
        {
            ListMessageDate.Add(dateTime);
        }
        public bool RuleException { get; set; }

    }
}
