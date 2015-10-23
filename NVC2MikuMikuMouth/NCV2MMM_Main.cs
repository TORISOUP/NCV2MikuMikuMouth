using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin;
using System.Diagnostics;
using System.Windows.Forms;

namespace NVC2MikuMikuMouth
{
    public class NCV2MMM_Main :  IPlugin
    {
        private IPluginHost pluginHost;
        private TCPManager tcpManager;
        private string broadcasterId = "";

        /// <summary>
        /// 起動時に自動実行
        /// </summary>
        public void AutoRun()
        {
            //待ち受け開始
            try
            {
                tcpManager = new TCPManager();
                tcpManager.ServerStart();
            }
            catch (Exception e)
            {
                ErrorLogger.OutputLog(Application.StartupPath + @"\NCV2MMM_error_log.txt", e.ToString());
                System.Windows.Forms.MessageBox.Show(e.ToString(), e.Message);
            }

            this.pluginHost.ReceivedComment += pluginHost_ReceivedComment;
            this.pluginHost.BroadcastConnected += pluginHost_BroadcastConnected;
            this.pluginHost.BroadcastDisConnected += pluginHost_BroadcastDisConnected;
            
        }

        void pluginHost_BroadcastDisConnected(object sender, EventArgs e)
        {
            broadcasterId = "";
        }

        void pluginHost_BroadcastConnected(object sender, EventArgs e)
        {
            //GetPlayerStatusから配信者IDを取得する
            broadcasterId = pluginHost.GetPlayerStatus().Stream.OwnerId;
        }

        void pluginHost_ReceivedComment(object sender, ReceivedCommentEventArgs e)
        {

            try
            {
                if (e.CommentDataList.Count > 0)
                {
                    //コメント取得時に変換して送信
                    var newLiveCommentData = e.CommentDataList.LastOrDefault();
                    if (newLiveCommentData == null) { return; }

                    //ユーザ設定リスト取得
                    var userList = this.pluginHost.GetUserSettingInPlugin().UserDataList;
                    //コメントのユーザがユーザ設定リストにあれば取得
                    var user = userList.FirstOrDefault(x => x.UserId == newLiveCommentData.UserId);

                    var commentInfo = new RequestDataPackage(newLiveCommentData,user, this.broadcasterId);
                    var jsonString = commentInfo.ToJson();
                    tcpManager.SendToAll(jsonString);
                }
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
                ErrorLogger.OutputLog(Application.StartupPath + @"\NCV2MMM_error_log.txt", ex.ToString());
                System.Windows.Forms.MessageBox.Show(ex.ToString(), ex.Message);
            }
        }

        public string Description
        {
            get { return "みくみくまうすへコメント情報を送信するプラグインです"; }
        }

        public IPluginHost Host
        {
            get
            {
                return this.pluginHost;
            }
            set
            {
                this.pluginHost = value;
            }
        }

        public bool IsAutoRun
        {
            get { return true; }
        }

        public string Name
        {
            get { return "NCV2MikuMikuMouth"; }
        }

        public void Run()
        {
        }

        public string Version
        {
            get { return "1.0b"; }
        }
    }
}
