using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Web.Script.Serialization;
using SocketServerForApp.Classes.Model;
using System.Reflection;

namespace SocketServerForApp.Classes.Sockets
{
    public class SocketClientListener
    {
        static JavaScriptSerializer jss = new JavaScriptSerializer();
        Socket listener;
        string clientId;
        Dictionary<string, Socket> dic_client;

        public SocketClientListener(Socket _listener, string _clientId, Dictionary<string, Socket> _dic_client)
        {
            listener = _listener;
            clientId = _clientId;
            dic_client = _dic_client;
        }

        #region 服務端監聽

        public void Listen<T>(Action<BaseSocketModel<T>> callback) where T : class
        {
            Listen(callback, null);
        }
        public void Listen<T>(Action<BaseSocketModel<T>> callback, Action onCompleted) where T : class
        {
            if (listener != null)
            {
                //start new thread
                Thread t = new Thread(() => { ListenStart(callback, onCompleted); });
                t.IsBackground = true;
                t.Start();
            }
        }

        private void ListenStart<T>(Action<BaseSocketModel<T>> callback, Action onCompleted) where T : class
        {
            bool isListening = true;
            byte[] byteFromClient = new byte[4096];
            while (isListening)
            {
                try
                {
                    if (listener != null && listener.Connected)
                    {
                        int length = listener.Receive(byteFromClient);
                        if (length >= 0)
                        {
                            string data = Encoding.UTF8.GetString(byteFromClient, 0, length);
                            object obj_data = jss.Deserialize(data, typeof(BaseSocketModel<T>));
                            if (obj_data != null)
                            {
                                BaseSocketModel<T> model = obj_data as BaseSocketModel<T>;
                                string clientId = model.clientId;
                                //若標誌斷開連接，斷開
                                if (model.disconnect)
                                {
                                    isListening = false;
                                    Remove();
                                    if (onCompleted != null)
                                        onCompleted();
                                }
                                //否則進行正常回調
                                else if (!string.IsNullOrWhiteSpace(clientId) && callback != null)
                                    callback(obj_data as BaseSocketModel<T>);
                            }
                        }
                    }
                }
                catch
                {
                    isListening = false;
                    Remove();
                }
            }
        }

        public void Remove()
        {
            if (listener != null)
            {
                if (dic_client != null && dic_client.Count > 0 && dic_client.ContainsKey(clientId))
                    dic_client.Remove(clientId);
                listener.Close();
                listener = null;
            }
        }
        #endregion


        #region 客戶端監聽

        public void ClientListen<T>(Action<BaseSocketResponseModel<T>> callback) where T : class
        {
            if (listener != null)
            {
                //start new thread
                Thread t = new Thread(() => { ClientListenStart(callback); });
                t.IsBackground = true;
                t.Start();
            }
        }

        private void ClientListenStart<T>(Action<BaseSocketResponseModel<T>> callback) where T : class
        {
            bool isListening = true;
            byte[] byteFromClient = new byte[4096];
            while (isListening)
            {
                try
                {
                    if (listener != null && listener.Poll(-1, SelectMode.SelectRead) && listener.Connected)
                    {
                        int length = listener.Receive(byteFromClient);
                        if (length > 0)
                        {
                            string data = Encoding.UTF8.GetString(byteFromClient, 0, length);
                            object obj_data = jss.Deserialize(data, typeof(BaseSocketResponseModel<T>));
                            if (obj_data != null)
                            {
                                BaseSocketResponseModel<T> model = obj_data as BaseSocketResponseModel<T>;
                                if (model.errorCode == 0 && callback != null)
                                    callback(obj_data as BaseSocketResponseModel<T>);
                            }
                        }
                        else
                        {
                            //socket鏈接已斷開
                            callback(new BaseSocketResponseModel<T>() { errorCode = 500 });
                        }
                    }
                }
                catch
                {
                    isListening = false;
                    Remove();
                }
            }
        }
        #endregion
    }
}
