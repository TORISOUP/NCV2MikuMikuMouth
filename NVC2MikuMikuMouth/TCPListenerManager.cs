using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NVC2MikuMikuMouth
{
    class TCPManager
    {
        int port = 50082;
        TcpListener listener;
        List<TcpClient> tcpClients;
        System.Text.Encoding encoding;
        Task taskListen;

        public TCPManager()
        {
            tcpClients = new List<TcpClient>();
            encoding = System.Text.Encoding.UTF8;
        }

        public void ServerStart()
        {
            try
            {
                Debug.Print("待受を開始しました");
                this.taskListen = Task.Factory.StartNew(() =>
                {
                    Accept();
                });
            }
            catch (Exception e)
            {
                Debug.Fail("Server Error", e.Message);
            }
        }


        /// <summary>
        /// 外部からの接続を待機する
        /// </summary>
        /// <returns></returns>
        Task Accept()
        {
            while (true)
            {
                this.listener = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), port);
                this.listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                client.NoDelay = true;
                lock (tcpClients)
                {
                    tcpClients.Add(client);
                }
                Debug.Print("接続{0}", ((IPEndPoint)(client.Client.RemoteEndPoint)).Address);
            }
        }

        public void Disconnect()
        {
            listener.Stop();
        }

        /// <summary>
        /// 全てのクライアントにMessageをブロードキャストする
        /// </summary>
        /// <param name="client_data"></param>
        /// <param name="message"></param>
        public void SendToAll(string message)
        {
            Task.Factory.StartNew(() =>
            {
                //接続が切れているクライアントは除去する
                lock (tcpClients)
                {
                    var closedClients = tcpClients.Where(x => !x.Connected).ToList();
                    closedClients.ForEach(x => tcpClients.Remove(x));

                    foreach (var client in tcpClients)
                    {
                        //接続が切れていないか再確認
                        if (!client.Connected) { continue; }
                        var ns = client.GetStream();
                        byte[] message_byte = encoding.GetBytes(message);
                        try
                        {
                            do
                            {
                                ns.Write(message_byte, 0, message_byte.Length);
                            } while (ns.DataAvailable);
                        }
                        catch (Exception e)
                        {
                            Debug.Print(e.Message);
                            if (!client.Connected)
                            {
                                client.Close();
                            }
                        }
                    }
                }
            });
        }
    }

}
