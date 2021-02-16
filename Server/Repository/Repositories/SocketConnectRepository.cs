using Model.Migrations;
using MongoDB.Driver;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class SocketConnectRepository
    {
        private readonly IMongoCollection<SocketConnect> _conn;

        public SocketConnectRepository(MongoConnection mongoDb)
        {
            _conn = mongoDb.MongoDbServer().GetCollection<SocketConnect>("SocketConnect");
        }

        /// <summary>
        /// 新增/更新 SocketConnect
        /// </summary>
        /// <param name="socketConnect"></param>
        /// <returns></returns>
        public async Task<bool> UpsertSocketConnectAsync(SocketConnect socketConnect)
        {
            var filter = Builders<SocketConnect>.Filter.Eq(x => x.ConnectId, socketConnect.ConnectId);

            var update = Builders<SocketConnect>.Update
                            .SetOnInsert(x => x.UsersId, socketConnect.UsersId)
                            .Set(x => x.ConnectId, socketConnect.ConnectId)
                            .Set(x => x.SocketConnectOnlineType, socketConnect.SocketConnectOnlineType)
                            .Set(x => x.LastDate, socketConnect.LastDate);

            var option = new UpdateOptions
            {
                IsUpsert = true
            };

            return (await _conn.UpdateOneAsync(filter, update, option)).IsAcknowledged;
        }

        /// <summary>
        /// 新增/更新 SocketConnect
        /// </summary>
        /// <param name="usersIds"></param>
        /// <returns></returns>
        public async Task<List<SocketConnect>> GetListSocketConnectByUsersIdsAsync(List<string> usersIds)
        {
            var filter = Builders<SocketConnect>.Filter.In(x => x.UsersId, usersIds);

            return await _conn.Find(filter).ToListAsync();
        }

        /// <summary>
        /// 取得使用者 By ConnectId
        /// </summary>
        /// <param name="connectId"></param>
        /// <returns></returns>
        public async Task<SocketConnect> GetListSocketConnectByConnectIdsAsync(string connectId)
        {
            var filter = Builders<SocketConnect>.Filter.Eq(x => x.ConnectId, connectId);

            return await _conn.Find(filter).FirstOrDefaultAsync();
        }

        
    }
}
