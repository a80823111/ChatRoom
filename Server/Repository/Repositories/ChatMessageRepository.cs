using Model.Migrations;
using MongoDB.Driver;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ChatMessageRepository
    {
        private readonly IMongoCollection<ChatMessage> _conn;

        public ChatMessageRepository(MongoConnection mongoDb)
        {
            _conn = mongoDb.MongoDbServer().GetCollection<ChatMessage>("ChatMessage");
        }

        /// <summary>
        /// 新增訊息
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        public async Task<bool> UpsertAsync(ChatMessage chatMessage)
        {
            var filter = Builders<ChatMessage>.Filter.Eq(x => x.ChatRoom, chatMessage.ChatRoom) &
                         Builders<ChatMessage>.Filter.Eq(x => x.BuketDate, chatMessage.BuketDate.Date);

            var update = Builders<ChatMessage>.Update
                            .SetOnInsert(x => x.ChatRoom, chatMessage.ChatRoom)
                            .SetOnInsert(x => x.BuketDate, chatMessage.BuketDate.Date);

            foreach(var chatContent in chatMessage.ChatContents)
            {
                update = update.Push(x => x.ChatContents, chatContent);
            }

            var option = new UpdateOptions
            {
                IsUpsert = true
            };

            return (await _conn.UpdateOneAsync(filter, update, option)).IsAcknowledged;
        }

        /// <summary>
        /// 取得訊息
        /// </summary>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
        public async Task<List<ChatMessage>> GetListChatMessageByChatRoomAsync(string chatRoom)
        {
            var filter = Builders<ChatMessage>.Filter.Eq(x => x.ChatRoom, chatRoom);

            return await _conn.Find(filter).ToListAsync();
        }

        /// <summary>
        /// 取得使用者進入過的群組
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        public async Task<List<ChatMessage>> GetListUsersEnterBeforeChatRoomAsync(string usersId)
        {
            var filter = Builders<ChatMessage>.Filter
                .ElemMatch(x => x.ChatContents,Builders<ChatContent>.Filter.Eq(y => y.UsersId,usersId));

            return await _conn.Find(filter)
                .Project(x => new ChatMessage { 
                    ChatRoom = x.ChatRoom,
                    ChatContents = x.ChatContents.Select(y => new ChatContent { UsersId = y.UsersId }).ToList()
                })
                .ToListAsync();
        }

        /// <summary>
        /// 取得群組成員
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetListChatRoomMemberAsync(string chatroom)
        {
            var filter = Builders<ChatMessage>.Filter
                .Eq(x => x.ChatRoom, chatroom);

            return (
                    await _conn.Find(filter)
                               .Project(x => x.ChatContents.Select(y => y.UsersId))
                               .ToListAsync()
                   )
                   .SelectMany(x => x)
                   .Distinct()
                   .ToList();
        }

    }

}
