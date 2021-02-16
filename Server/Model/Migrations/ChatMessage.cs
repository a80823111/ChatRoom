using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Migrations
{
    public class ChatMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ChatRoom { get; set; }

        // <summary>
        /// 訊息內容
        /// </summary>
        public List<ChatContent> ChatContents { get; set; }

        /// <summary>
        /// 存儲桶日期 YYYY-mm-dd (建立日期)
        /// </summary>
        public DateTime BuketDate { get; set; }
    }

    /// <summary>
    /// 訊息內容
    /// </summary>
    public class ChatContent
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        /// <summary>
        /// 訊息發送者
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsersId { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
