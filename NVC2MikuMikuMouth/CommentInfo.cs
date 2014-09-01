using NicoLibrary.NicoLiveData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NVC2MikuMikuMouth
{
    [DataContract]
    class CommentInfo
    {
        DataContractJsonSerializer jsonSerializer;
        public CommentInfo(LiveCommentData liveCommentData ,string broadcasterId )
        {
            jsonSerializer = new DataContractJsonSerializer(typeof(CommentInfo));
            this.Name = liveCommentData.Name;
            this.NickName = "";
            this.Anonymity = liveCommentData.IsAnonymity;
            this.IsCaster = liveCommentData.UserId == broadcasterId;
            this.Message = liveCommentData.Comment;
            this.No = Int32.Parse(liveCommentData.No);
            this.Premium = liveCommentData.Premium;
            this.ProfName = "";
            this.UserId = liveCommentData.UserId;
            this.Hiragana = MojiConverter.ConvertToHiragana(liveCommentData.Comment);
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

        [DataMember]
        public string Name;
        [DataMember]
        public string NickName;
        [DataMember]
        public bool Anonymity;
        [DataMember]
        public bool IsCaster;
        [DataMember]
        public string Message;
        [DataMember]
        public int No;
        [DataMember]
        public int Premium;
        [DataMember]
        public string ProfName;
        [DataMember]
        public string UserId;
        [DataMember]
        public string Hiragana;
    }
}
