using Model.Enum.SocketConnectType;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Migrations
{
    /// <summary>
    /// Socket 訊息
    /// </summary>
    public class SocketConnect
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UsersId { get; set; }

        public string ConnectId { get; set; }

        public SocketConnectOnlineType SocketConnectOnlineType { get; set; }

        public DateTime LastDate { get; set; }
    }
}
