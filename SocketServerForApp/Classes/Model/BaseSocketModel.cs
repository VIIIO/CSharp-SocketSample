using System;
namespace SocketServerForApp.Classes.Model
{
    public class BaseSocketModel<T> where T : class
    {
        public string controller { get; set; }
        public string action { get; set; }

        public string clientId { get; set; }

        public bool disconnect { get; set; }

        public T data { get; set; }
    }

    public class BaseSocketModel : BaseSocketModel<string>
    {
        
    }
}
