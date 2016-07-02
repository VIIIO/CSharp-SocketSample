using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using SocketServerForApp.Classes.Model;
using SocketServerForApp.Classes.Global;
using SocketServerForApp.Classes.Helper;

namespace SocketServerForApp.Classes.Sockets
{
    public class BroadCast
    {
        /// <summary>
        /// 發送給全部人
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dic_client"></param>
        /// <param name="fromClient"></param>
        public static void Push(string message, Dictionary<string, Socket> dic_client, string fromClient = null)
        {
            Push(message, CmdTypeEnum.Normal, dic_client, fromClient);
        }

        /// <summary>
        /// 發送給全部人
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dic_client"></param>
        /// <param name="fromClient"></param>
        public static void Push<T>(T message, CmdTypeEnum cmdType, Dictionary<string, Socket> dic_client, string fromClient = null) where T : class
        {
            foreach (var client in dic_client.Values)
            {
                BaseSocketResponseModel<T> model = new BaseSocketResponseModel<T>();
                model.fromClient = fromClient;
                model.data = message;
                model.cmdType = cmdType.ToString();

                byte[] byteMessage = new byte[4096];
                byteMessage = Encoding.UTF8.GetBytes(JSONHelper.jss.Serialize(model));
                try
                {
                    client.Send(byteMessage);
                }
                catch
                {
                    client.Close();
                }
            }
        }

        /// <summary>
        /// 發送給指定人
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientId"></param>
        /// <param name="cmdType"></param>
        /// <param name="message"></param>
        /// <param name="dic_client"></param>
        /// <param name="fromClient"></param>
        public static void PushToClientId<T>(string clientId, CmdTypeEnum cmdType, T message, Dictionary<string, Socket> dic_client, string fromClient = null) where T : class
        {
            if (dic_client.ContainsKey(clientId))
            {
                Socket client = dic_client[clientId];
                BaseSocketResponseModel<T> model = new BaseSocketResponseModel<T>();
                model.fromClient = fromClient;
                model.data = message;
                model.cmdType = cmdType.ToString();

                byte[] byteMessage = new byte[4096];
                byteMessage = Encoding.UTF8.GetBytes(JSONHelper.jss.Serialize(model));
                try
                {
                    client.Send(byteMessage);
                }
                catch
                {
                    client.Close();
                }
            }
        }
    }
}
