using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServiceCore
{
    /// <summary>
    /// aynı pc e farklı client üretip username göre ayırt etmek 
    /// ve mesajı username ile ilişkilendirmek için.
    /// </summary>
    [Serializable]
    public class ClientServerMessage
    {
        public string UserName { get; set; }
        public string Message { get; set; }
       
    }
}
