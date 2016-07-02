using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerForApp.Classes.Model
{
    public class BaseSocketResponseModel<T> where T : class
    {
        public BaseSocketResponseModel()
        {
            errorCode = 0;
            errorMsg = string.Empty;
            fromClient = string.Empty;
            cmdType = string.Empty;
            data = default(T);
        }
        public int errorCode { get; set; }
        public string errorMsg { get; set; }
        public string cmdType { get; set; }
        public string fromClient { get; set; }
        public T data { get; set; }
    }

    public class BaseSocketResponseModel : BaseSocketResponseModel<string>
    {

    }
}
