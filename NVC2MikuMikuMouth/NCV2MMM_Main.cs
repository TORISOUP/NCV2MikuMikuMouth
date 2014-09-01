﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin;
using System.Diagnostics;

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
            tcpManager = new TCPManager();
            tcpManager.ServerStart();

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
            //コメント取得時に変換して送信
            var newLiveCommentData = e.CommentDataList.Last();
            var commentInfo = new CommentInfo(newLiveCommentData, this.broadcasterId);
            var jsonString = commentInfo.ToJson();
            tcpManager.SendToAll(jsonString);
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
