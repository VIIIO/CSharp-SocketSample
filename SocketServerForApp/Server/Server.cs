using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web.Script.Serialization;
using SocketServerForApp.Classes.Model;
using SocketServerForApp.Classes.Sockets;
using SocketServerForApp.Classes.Helper;
using SocketServerForApp.Classes.Global;

namespace SocketServerForApp.Server
{
    public partial class Server : Form
    {
        #region 初始化

        private volatile bool isActivate = false; //volatile指示可以由多个同时执行的线程修改
        private Socket serverSocket = null;
        private Dictionary<string, Socket> dic_client = new Dictionary<string, Socket>();
        private Thread t;//服務端監聽線程
        public Server()
        {
            InitializeComponent();
        }
        #endregion

        #region 同步Socket
        private void syncSocketExample()
        {
            btn_control.Enabled = false;
            if (!isActivate)
            {
                //啟動
                serverOn();
            }
            else
            {
                //停止
                serverOff();
            }
            btn_control.Enabled = true;
        }
        private void serverOn()
        {
            dic_client = new Dictionary<string, Socket>();
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("192.168.88.126"), 8080));
            serverSocket.Listen(0);

            showMsg("正在啟動服務...");
            t = new Thread(serverStartListening);
            t.IsBackground = true;
            t.Start();

            // Loop until worker thread activates.
            while (!t.IsAlive) ;

            showMsg("服務已啟動");
            changeControlText(btn_control, @"停止");
        }
        private void serverStartListening()
        {
            isActivate = true;
            Socket client = default(Socket);
            while (isActivate)
            {
                showMsg("正在等待客戶端連接...");
                try
                {
                    client = serverSocket.Accept();
                    showMsg("檢測到新的接入請求.");
                }
                catch (Exception ex)
                {
                    if (ex is SocketException)
                    {
                        SocketException socketEx = ex as SocketException;
                        if (socketEx.SocketErrorCode == SocketError.Interrupted)
                            showMsg("ServerSocket已停止監聽。");
                        else
                            showMsg(ex.Message);
                    }
                    else
                        showMsg(ex.Message);
                }
                byte[] byteFrom = new byte[4096];
                if (client != null && client.Connected)
                {
                    if (client.Available < 0) continue;
                    int length = client.Receive(byteFrom);
                    if (length >= 0)
                    {
                        string data = Encoding.UTF8.GetString(byteFrom, 0, length);
                        object obj_data = JSONHelper.jss.Deserialize(data, typeof(BaseSocketModel<object>));
                        if (obj_data != null)
                        {
                            BaseSocketModel<object> model = obj_data as BaseSocketModel<object>;
                            #region 驗證連接并添加至用戶列表
                            if ("account".Equals(model.controller.ToLower()) && "login".Equals(model.action.ToLower()))
                            {
                                //進行廣播
                                BroadCast.Push(model.clientId + "已連接", dic_client, model.clientId);
                                //服務端本地通知
                                showMsg(model.clientId + "已接入");
                                //增加客戶端監聽
                                dic_client[model.clientId] = client;
                                #region 發送在線用戶列表***測試***
                                List<string> list_user = new List<string>();
                                foreach (var key in dic_client.Keys)
                                    list_user.Add(key);
                                sendUserList();
                                //BroadCast.PushToClientId(model.clientId, Classes.Global.CmdTypeEnum.ChatRoomUserList, list_user, dic_client);
                                #endregion
                                SocketClientListener listener = new SocketClientListener(client, model.clientId, dic_client);
                                listener.Listen<object>((callbackData) =>
                                {
                                    //可以儲存數據到數據庫等
                                    showMsg(string.Format(@"{0}-{1}發送了消息：{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), callbackData.clientId, callbackData.data));
                                    //廣播給所有客戶端
                                    BroadCast.Push(callbackData.data.ToString(), dic_client, model.clientId);
                                }, () => {
                                    //斷開連接
                                    dic_client.Remove(model.clientId);
                                    sendUserList();
                                    showMsg(model.clientId + "已斷開");
                                    BroadCast.Push(model.clientId + "已離開", dic_client);
                                });
                            }
                            #endregion
                        }
                    }
                }
            }
        }
        private void serverOff()
        {
            showMsg("正在關閉服務...");
            if (serverSocket != null)
            {
                BroadCast.Push("服務器已關閉", dic_client);
                isActivate = false;
                foreach (var item in dic_client.Values)
                {
                    try
                    {
                        item.Shutdown(SocketShutdown.Both);
                        item.Close();
                    }
                    catch (Exception) { }
                }
                dic_client.Clear();
                try
                {
                    serverSocket.Shutdown(SocketShutdown.Both);
                }
                catch
                {

                }
                serverSocket.Close();
                serverSocket = null;
                // Use the Join method to block the current thread 
                // until the thread terminates.
                t.Join();
                changeControlText(btn_control, @"啟動");
            }
            showMsg("服務已關閉.");
        }
        private void sendUserList()
        {
            #region 發送在線用戶列表***測試***
            List<string> list_user = new List<string>();
            foreach (var key in dic_client.Keys)
                list_user.Add(key);
            BroadCast.Push(list_user, Classes.Global.CmdTypeEnum.ChatRoomUserList, dic_client);
            #endregion
        }
        #endregion

        #region 異步Socket
        /// <summary>
        /// 很關鍵，如果你不了解它可以把它当做线程锁
        /// </summary>
        ManualResetEvent allDone = new ManualResetEvent(false);
        private void asyncSocketExample()
        {
            btn_control.Enabled = false;
            if (!isActivate)
            {
                //啟動
                asyncServerOn();
            }
            else
            {
                //停止
                asyncServerOff();
            }
            btn_control.Enabled = true;
        }
        private void asyncServerOn()
        {
            dic_client = new Dictionary<string, Socket>();
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //可能引发端口被占用异常
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(GlobalConfig.serverIp), GlobalConfig.serverPort));
            serverSocket.Listen(0);
            
            showMsg("正在啟動服務...");
            //其實異步Socket不用開啟獨立線程，這裡只是為了與同步的方法保持相同結構
            t = new Thread(asyncServerStartListening);
            t.IsBackground = true;
            t.Start();

            // Loop until worker thread activates.
            while (!t.IsAlive) ;

            showMsg("服務已啟動");
            changeControlText(btn_control, @"停止");
        }
        private void asyncServerStartListening()
        {
            isActivate = true;
            try
            {
                while (isActivate)
                {
                    allDone.Reset();//重置信號
                    showMsg("正在等待客戶端連接...");
                    serverSocket.BeginAccept(new AsyncCallback(ar =>
                    {
                        if (serverSocket != null)
                        {
                            showMsg("檢測到新的接入請求.");
                            // Signal thread to continue.     
                            allDone.Set();
                            Socket client = serverSocket.EndAccept(ar);//serverSocket也可以用Socket serverSocket = (Socket)ar.AsyncState;獲取
                            byte[] byteFrom = new byte[4096];
                            //開始接收數據
                            client.BeginReceive(byteFrom, 0, 4096, 0, new AsyncCallback(rAr =>
                            {
                                //client也可以用Socket client = (Socket)rAr.AsyncState;獲取，當然也可以將byteFrom、Socket包裝到一個Object中分開獲取
                                #region 解析接收內容（解析之後每一個Client Socket都可以用BeginReceive/Receive來獲取數據，使用方法不再贅述）
                                if (client != null && client.Connected)
                                {
                                    int length = client.EndReceive(rAr);
                                    if (length >= 0)
                                    {
                                        string data = Encoding.UTF8.GetString(byteFrom, 0, length);
                                        object obj_data = JSONHelper.jss.Deserialize(data, typeof(BaseSocketModel<object>));
                                        if (obj_data != null)
                                        {
                                            BaseSocketModel<object> model = obj_data as BaseSocketModel<object>;
                                            #region 驗證連接并添加至用戶列表
                                            if ("account".Equals(model.controller.ToLower()) && "login".Equals(model.action.ToLower()))
                                            {
                                                //進行廣播
                                                BroadCast.Push(model.clientId + "已連接", dic_client, model.clientId);
                                                //服務端本地通知
                                                showMsg(model.clientId + "已接入");
                                                //增加客戶端監聽
                                                dic_client[model.clientId] = client;
                                                #region 發送在線用戶列表***測試***
                                                List<string> list_user = new List<string>();
                                                foreach (var key in dic_client.Keys)
                                                    list_user.Add(key);
                                                sendUserList();
                                                //BroadCast.PushToClientId(model.clientId, Classes.Global.CmdTypeEnum.ChatRoomUserList, list_user, dic_client);
                                                #endregion
                                                SocketClientListener listener = new SocketClientListener(client, model.clientId, dic_client);
                                                listener.Listen<object>((callbackData) =>
                                                {
                                                    //可以儲存數據到數據庫等
                                                    showMsg(string.Format(@"{0}-{1}發送了消息：{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), callbackData.clientId, callbackData.data));
                                                    //廣播給所有客戶端
                                                    BroadCast.Push(callbackData.data.ToString(), dic_client, model.clientId);
                                                }, () =>
                                                {
                                                    //斷開連接
                                                    dic_client.Remove(model.clientId);
                                                    sendUserList();
                                                    showMsg(model.clientId + "已斷開");
                                                    BroadCast.Push(model.clientId + "已離開", dic_client);
                                                });
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion
                            }), client);
                        }
                        else
                        {
                            //此時isActivate == false
                            allDone.Set();//Continue to break while-loop
                        }
                    }), serverSocket);
                    allDone.WaitOne();//等待解鎖信號Set
                }
            }
            catch (Exception ex)
            {
                if (ex is SocketException)
                {
                    SocketException socketEx = ex as SocketException;
                    if (socketEx.SocketErrorCode == SocketError.Interrupted)
                        showMsg("ServerSocket已停止監聽。");
                    else
                        showMsg(ex.Message);
                }
                else
                    showMsg(ex.Message);
            }
        }
        private void asyncServerOff()
        {
            showMsg("正在關閉服務...");
            if (serverSocket != null)
            {
                BroadCast.Push("服務器已關閉", dic_client);
                isActivate = false;
                foreach (var item in dic_client.Values)
                {
                    try
                    {
                        item.Shutdown(SocketShutdown.Both);
                        item.Close();
                    }
                    catch (Exception) { }
                }
                dic_client.Clear();
                serverSocket.Close();
                serverSocket = null;
                // Use the Join method to block the current thread 
                // until the thread terminates.
                t.Join();
                changeControlText(btn_control, @"啟動");
            }
            showMsg("服務已關閉.");
        }
        #endregion

        #region 控件觸發
        private void btn_control_Click(object sender, EventArgs e)
        {
            //syncSocketExample();//開啟同步Socket服務
            asyncSocketExample();//開啟異步Socket服務
        }
        private void changeControlText(Control ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.BeginInvoke(new Action(() => ctrl.Text = text));
            }
            else
            {
                ctrl.Text = text;
            }
        }

        private void showMsg(object msg)
        {
            if (txt_content.InvokeRequired)
            {
                txt_content.BeginInvoke(new Action(() => txt_content.Text = (msg.ToString() + Environment.NewLine + txt_content.Text)));
            }
            else
            {
                txt_content.Text = (msg.ToString() + Environment.NewLine + txt_content.Text);
            }
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            serverOff();
            this.Dispose();
        }
        #endregion
    }
}
