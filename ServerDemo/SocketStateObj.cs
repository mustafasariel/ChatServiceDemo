using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerDemo
{
    /// <summary>
    /// client ın server  tarafından yönetileceği durumlar.
    /// </summary>
    public enum SocketState
    {
        None,
        warning,
        Disconnect
    }

    /// <summary>
    /// Gelen her bağlantı nesnesini ve  mesaj zamanlarını tutar.
    /// son mesaj zamanı ile bir önceki mesaj zamanı arasındaki farkı ölçemk için liste oluşturuldu.
    /// </summary>
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
        public bool RuleException { get; set; }// uyarı aldıysa true

    }
}
