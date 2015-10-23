using NicoLibrary.NicoLiveData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NVC2MikuMikuMouth
{
    [DataContract]
    class RequestDataPackage
    {
        DataContractJsonSerializer jsonSerializer;
        public RequestDataPackage(LiveCommentData liveCommentData,Plugin.UserSettingInPlugin.UserData user,string broadcasterId )
        {
            jsonSerializer = new DataContractJsonSerializer(typeof(RequestDataPackage));
            this.name = user != null ? user.NickName : ""; //ユーザ設定があるならそこの名前を設定
            this.isInterrupted = liveCommentData.UserId == broadcasterId;
            this.text = liveCommentData.Comment;
            this.tag = liveCommentData.Mail;
        }

        public string ToJson()
        {
            string result = "";
            using (var stream = new MemoryStream())
            {
                jsonSerializer.WriteObject(stream, this);

                stream.Position = 0;
                var reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// キャラクタのアニメーション
        /// </summary>
        [DataMember]
        public string emotion;
        /// <summary>
        /// コメントのカラー
        /// </summary>
        [DataMember]
        public string tag;
        /// <summary>
        /// 読み上げるメッセージ
        /// </summary>
        [DataMember]
        public string text;
        /// <summary>
        /// コメント投稿者
        /// </summary>
        [DataMember]
        public string name;
        /// <summary>
        /// 運営コメントかどうか
        /// </summary>
        [DataMember]
        public bool isInterrupted;
    }
}
