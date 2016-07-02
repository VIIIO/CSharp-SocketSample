using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketServerForApp.Classes.Model;
using SocketServerForApp.Classes.Sockets;
using SocketServerForApp.Classes.Helper;
using SocketServerForApp.Classes.Global;

namespace SocketServerForApp.Client
{
    public partial class Client : Form
    {

        Socket clientSocket = null;
        private bool IsListening = false;
        private string currentAccountName = string.Empty;
        public Client()
        {
            InitializeComponent();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_account.Text.Trim()) && (clientSocket == null || !clientSocket.Connected))
            {
                string st_account = txt_account.Text.Trim();
                IsListening = true;
                showMsg("正在進入聊天室...");
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.BeginConnect(GlobalConfig.serverIp, GlobalConfig.serverPort, (ar) => {
                    //ar : IAsyncResult
                    if (ar.IsCompleted)
                    {
                        //login
                        BaseSocketModel model = new BaseSocketModel();
                        model.controller = "account";
                        model.action = "login";
                        model.clientId = st_account;
                        model.data = string.Empty;
                        currentAccountName = st_account;

                        byte[] byteSend = new byte[4096];
                        byteSend = Encoding.UTF8.GetBytes(JSONHelper.jss.Serialize(model));
                        if (clientSocket != null && clientSocket.Connected)
                        {
                            clientSocket.Send(byteSend);
                            SocketClientListener listener = new SocketClientListener(clientSocket, st_account, null);
                            listener.ClientListen<object>((message) => {
                                if (message.errorCode == 500)
                                {
                                    //***斷線處理，但是這裡只能處理到服務端斷線的情況，如果是網絡原因則不能處理到
                                    //***真正的斷線檢測可以使用心跳包/TCP Keep-Alive等機制
                                    //***另外還要加入斷線重連功能，才算是完整的Socket處理
                                    showMsg("已斷線");
                                    clientSocket.Shutdown(SocketShutdown.Both);
                                    clientSocket.Close();
                                    clientSocket = null;
                                    IsListening = false;
                                }
                                else
                                {
                                    CmdTypeEnum cmdType = (CmdTypeEnum)Enum.Parse(typeof(CmdTypeEnum), message.cmdType);
                                    switch (cmdType)
                                    {
                                        case CmdTypeEnum.Normal: //普通消息
                                            if (!string.IsNullOrEmpty(message.fromClient))
                                                showMsg(message.fromClient + " : " + message.data.ToString());
                                            else
                                                showMsg(message.data.ToString());
                                            break;
                                        case CmdTypeEnum.ChatRoomUserList://在線列表
                                            List<string> list = new List<string>();
                                            object[] arr_obj = message.data as object[];
                                            foreach (var item in arr_obj)
                                                list.Add(item.ToString());
                                            if (list != null && list.Count > 0)
                                            {
                                                cbo_account.BeginInvoke(new Action(() =>
                                                {
                                                    cbo_account.DataSource = list;
                                                }));
                                            }
                                            showMsg("在線列表已更新");
                                            break;
                                    }
                                }
                            });
                        }
                        else
                        {
                            showMsg("無法連接到服務器");
                        }
                    }
                }, null);
            }
            else
            {
                showMsg("請先設定暱稱");
            }

        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_message.Text.Trim()))
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    byte[] bytesSend = Encoding.UTF8.GetBytes(
                                           JSONHelper.jss.Serialize(
                                              new BaseSocketModel()
                                              {
                                                  clientId = currentAccountName,
                                                  data = txt_message.Text.Trim()
                                              }));
                    clientSocket.Send(bytesSend);
                    txt_message.Text = string.Empty;
                }
                else
                {
                    showMsg("未连接服务器或者服务器已關閉，请重新登錄~");
                }
            }
            else
            {
                
            }
        }


        private void showMsg(object msg)
        {
            if (txt_content.InvokeRequired)
            {
                txt_content.BeginInvoke(new Action(() => {
                    privateShowMsg(msg);
                }));
            }
            else
            {
                privateShowMsg(msg);
            }
        }

        private void privateShowMsg(object msg)
        {
            if (!string.IsNullOrWhiteSpace(txt_content.Text))
                txt_content.Text += Environment.NewLine;
            txt_content.Text += msg.ToString();
            txt_content.SelectionLength = txt_content.Text.Length;
            txt_content.ScrollToCaret();
        }

        private void btn_disconnect_Click(object sender, EventArgs e)
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                //通知服務端
                byte[] byteSend = new byte[4096];
                byteSend = Encoding.UTF8.GetBytes(
                               JSONHelper.jss.Serialize(
                                   new BaseSocketModel()
                                   {
                                       disconnect = true
                                   }));
                clientSocket.Send(byteSend);
                clientSocket.Close();
                clientSocket = null;
                IsListening = false;
                showMsg("已退出聊天室");
            }
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            btn_disconnect_Click(null, null);
            this.Dispose();
        }
    }
}
